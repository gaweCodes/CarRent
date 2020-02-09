<template>
  <div class="container">
    <h1>Kunden</h1>
    <hr />
    <loading v-if="customerRd.isLoading()" />
    <alert v-if="customerRd.hasError()">
      <p>{{customerRd.getError()}}</p>
    </alert>
    <div class="row">
      <div class="col-md-12">
        <div class="form-group">
          <fieldset>
            <legend>Erstellen</legend>
            <label for="newFirstName">Vorname</label>&nbsp;
            <input type="text" id="newFirstName" required v-model="newCustomer.firstName" />&nbsp;
            <label for="newLastName">Nachname</label>&nbsp;
            <input type="text" id="newLastName" required v-model="newCustomer.lastName" />
            <br />
            <label for="newAddress">Adresse</label>
            <br />
            <textarea id="newAddress" rows="3" cols="100" required v-model="newCustomer.address" />
            <br />
            <button type="button" class="btn btn-primary" @click="add">
              <em class="fas fa-plus" /> Erstellen
            </button>
          </fieldset>
        </div>
      </div>
    </div>
    <br />
    <div class="row">
      <div class="col-md-12" v-if="customerRd.hasData()">
        <fieldset>
          <legend>Übersicht</legend>
          <div v-for="(customer, idx) in customerRd.getData()" :key="customer.id" class="entry">
            <div
              class="form-group"
              :style="idx % 2 === 0 ? 'background-color: white;' : 'background-color: lightgray;'"
            >
              <label>Kundennummer</label>&nbsp;
              <input class="form-control" type="text" readonly v-model="customer.id" />
              <br />
              <label :for="customer.id + 'FirstName'">Vorname</label>&nbsp;
              <input
                type="text"
                :id="customer.id + 'FirstName'"
                required
                v-model="customer.firstName"
                @input="update(customer)"
              />&nbsp;
              <label :for="customer.id + 'LastName'">Nachname</label>&nbsp;
              <input
                type="text"
                :id="customer.id + 'LastName'"
                required
                v-model="customer.lastName"
                @input="update(customer)"
              />
              <br />
              <label :for="customer.id + 'Address'">Adresse</label>
              <br />
              <textarea
                :id="customer.id + 'Address'"
                rows="3"
                cols="100"
                required
                v-model="customer.address"
                @input="update(customer)"
              />
              <br />
              <button type="button" class="btn btn-danger" @click="remove(customer.id)">
                <em class="fas fa-trash" />
              </button>
            </div>
          </div>
        </fieldset>
      </div>
    </div>
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
    return { customerRd: RemoteData.notAsked<ICustomer[], Error>(), newCustomer: {} as ICustomer };
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
      this.newCustomer.firstName = '';
      this.newCustomer.lastName = '';
      this.newCustomer.address = '';
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
      await axios.put('/api/customer/' + updateObj.id, updateObj);
    },
    async remove(id: string) {
      await axios.delete('/api/customer/' + id);
      this.loadData();
    }
  }
});
</script>
