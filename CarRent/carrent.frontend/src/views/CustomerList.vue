<template>
  <div class="container">
    <h1>Kunden</h1>
    <hr />
    <alert v-if="customerRd.hasError()">
      <p>{{customerRd.getError()}}</p>
    </alert>
    <loading v-if="customerRd.isLoading()" />
    <div class="row">
      <div class="col-md-6">
        <div class="form-group">
          <fieldset>
            <legend>Erstellen</legend>
            <input
              type="text"
              id="newFirstName"
              required
              v-model="newCustomer.firstName"
              placeholder="Vorname"
              class="form-control"
            />
            <br />
            <input
              type="text"
              id="newLastName"
              required
              v-model="newCustomer.lastName"
              placeholder="Nachname"
              class="form-control"
            />
            <br />
            <textarea
              id="newAddress"
              rows="3"
              cols="100"
              required
              v-model="newCustomer.address"
              placeholder="Adresse"
              class="form-control"
            />
            <br />
            <button type="button" class="btn btn-primary" @click="add">
              <em class="fas fa-plus" />&nbsp;Erstellen
            </button>
          </fieldset>
        </div>
      </div>
      <div class="col-md-6">
        <div class="form-group">
          <fieldset>
            <legend>Suche</legend>
            <input
              type="text"
              v-model="searchObject.firstName"
              placeholder="Nach Vornamen suchen"
              class="form-control"
            />
            <br />
            <input
              type="text"
              v-model="searchObject.lastName"
              placeholder="Nach Nachnamen suchen"
              class="form-control"
            />
            <br />
            <textarea
              rows="3"
              v-model="searchObject.address"
              placeholder="Nach Adresse suchen"
              class="form-control"
            />
            <br />
            <button type="button" class="btn btn-primary" @click="search()">
              <em class="fas fa-search" />&nbsp;Suchen
            </button>
          </fieldset>
        </div>
      </div>
    </div>
    <br />
    <fieldset v-if="customerRd.hasData()">
      <legend>Übersicht</legend>
      <div class="row">
        <div class="col-md-3">
          <label>Kunden Nr.</label>
        </div>
        <div class="col-md-3">
          <label>Vorname</label>
        </div>
        <div class="col-md-3">
          <label>Nachname</label>
        </div>
        <div class="col-md-3">
          <label>Adresse</label>
        </div>
      </div>
      <div
        v-for="(customer, idx) in customerRd.getData()"
        :key="customer.id"
        :class="idx % 2 === 0 ? 'row entry even' : 'row entry odd'"
      >
        <div class="col-md-3 form-group">
          <input class="form-control" type="text" readonly v-model="customer.id" />
        </div>
        <div class="col-md-3 form-group">
          <input
            type="text"
            :id="customer.id + 'FirstName'"
            required
            v-model="customer.firstName"
            class="form-control"
            @input="update(customer)"
          />
        </div>
        <div class="col-md-3 form-group">
          <input
            type="text"
            :id="customer.id + 'LastName'"
            required
            v-model="customer.lastName"
            class="form-control"
            @input="update(customer)"
          />
        </div>
        <div>
          <textarea
            :id="customer.id + 'Address'"
            rows="3"
            required
            v-model="customer.address"
            class="form-control"
            @input="update(customer)"
          />
        </div>
        <div class="col-md-2 form-group">
          <button type="button" class="btn btn-danger form-control" @click="remove(customer.id)">
            <em class="fas fa-trash" />&nbsp;Löschen
          </button>
        </div>
      </div>
    </fieldset>
  </div>
</template>
<script lang="ts">
import Vue from 'vue';
import axios from 'axios';
import { RemoteData } from '@/helpers/RemoteData';
import Loading from '@/components/Loading.vue';
import Alert from '@/components/Alert.vue';
import { ICustomer } from '@/models/ICustomer';

export default Vue.extend({
  components: { Loading, Alert },
  data() {
    return {
      customerRd: RemoteData.notAsked<ICustomer[], Error>(),
      newCustomer: {} as ICustomer,
      searchObject: { firstName: undefined, lastName: undefined, address: undefined }
    };
  },
  created() {
    this.loadData();
  },
  methods: {
    loadData() {
      axios
        .get('/api/customer')
        .then(res => {
          this.customerRd = RemoteData.success<ICustomer[], Error>(res.data);
        })
        .catch(e => {
          this.customerRd = RemoteData.failure<ICustomer[], Error>(e);
        });
    },
    async add() {
      if (
        (document.getElementById('newFirstName') as HTMLFormElement).reportValidity() === false ||
        (document.getElementById('newLastName') as HTMLFormElement).reportValidity() === false ||
        (document.getElementById('newAddress') as HTMLFormElement).reportValidity() === false
      ) {
        return;
      }
      const uuidv1 = require('uuid/v1');
      this.newCustomer.id = uuidv1();
      await axios.post('/api/customer', this.newCustomer);
      this.newCustomer = {} as ICustomer;
      this.loadData();
    },
    async update(updateObj: ICustomer) {
      if (
        (document.getElementById(
          updateObj.id + 'FirstName'
        ) as HTMLFormElement).reportValidity() === false ||
        (document.getElementById(updateObj.id + 'LastName') as HTMLFormElement).reportValidity() ===
          false ||
        (document.getElementById(updateObj.id + 'Address') as HTMLFormElement).reportValidity() ===
          false
      ) {
        return;
      }
      await axios.put('/api/customer', updateObj);
    },
    async remove(id: string) {
      await axios.delete('/api/customer/' + id);
      this.loadData();
    },
    search() {
      let query = '';
      if (
        this.searchObject.firstName &&
        !this.searchObject.lastName &&
        !this.searchObject.address
      ) {
        query = '?firstName=' + this.searchObject.firstName;
      } else if (
        !this.searchObject.firstName &&
        this.searchObject.lastName &&
        !this.searchObject.address
      ) {
        query = '?lastName=' + this.searchObject.lastName;
      } else if (
        !this.searchObject.firstName &&
        !this.searchObject.lastName &&
        this.searchObject.address
      ) {
        query = '?address=' + this.searchObject.address;
      } else if (
        this.searchObject.firstName &&
        this.searchObject.lastName &&
        !this.searchObject.address
      ) {
        query =
          '?firstName=' + this.searchObject.firstName + '&lastName=' + this.searchObject.lastName;
      } else if (
        this.searchObject.firstName &&
        !this.searchObject.lastName &&
        this.searchObject.address
      ) {
        query =
          '?firstName=' + this.searchObject.firstName + '&address' + this.searchObject.address;
      } else if (
        !this.searchObject.firstName &&
        this.searchObject.lastName &&
        this.searchObject.address
      ) {
        query = '?lastName=' + this.searchObject.lastName + '&address' + this.searchObject.address;
      } else if (
        this.searchObject.firstName &&
        this.searchObject.lastName &&
        this.searchObject.address
      ) {
        query =
          '?firstName=' +
          this.searchObject.firstName +
          '&lastName=' +
          this.searchObject.lastName +
          '&address' +
          this.searchObject.address;
      }
      axios
        .get('/api/customer/search/' + query)
        .then(res => {
          this.customerRd = RemoteData.success<ICustomer[], Error>(res.data);
        })
        .catch(e => {
          this.customerRd = RemoteData.failure<ICustomer[], Error>(e);
        });
    }
  }
});
</script>
