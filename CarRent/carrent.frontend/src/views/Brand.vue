<template>
  <div class="container">
    <h1>Marken</h1>
    <hr />
    <loading v-if="brandRd.isLoading()" />
    <alert v-if="brandRd.hasError()">
      <p>{{brandRd.getError()}}</p>
    </alert>
    <div class="row">
      <div class="col-md-6">
        <div class="form-group">
          <fieldset>
            <legend>Erstellen</legend>
            <input
              type="text"
              id="new"
              class="form-control"
              required
              v-model="newBrand.title"
              placeholder="Name"
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
              class="form-control"
              type="text"
              v-model="searchText"
              placeholder="Nach Namen suchen"
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
    <fieldset v-if="brandRd.hasData()">
      <legend>Übersicht</legend>
      <div class="row">
        <div class="col-md-6">
          <label>Name</label>
        </div>
      </div>
      <div
        v-for="(brand, idx) in brandRd.getData()"
        :key="brand.id"
        :class="idx % 2 === 0 ? 'row entry even' : 'row entry odd'"
      >
        <div class="col-md-6 form-group">
          <input
            type="text"
            required
            :id="brand.id"
            v-model="brand.title"
            @input="update(brand)"
            class="form-control"
          />
        </div>
        <div class="col-md-6 form-group"></div>
        <div class="col-md-2 form-group">
          <button type="button" class="btn btn-danger form-control" @click="remove(brand.id)">
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
import { IBrand } from '@/models/IBrand';

export default Vue.extend({
  components: { Loading, Alert },
  data() {
    return {
      brandRd: RemoteData.notAsked<IBrand[], Error>(),
      newBrand: {} as IBrand,
      searchText: ''
    };
  },
  created() {
    this.loadData();
  },
  methods: {
    loadData() {
      axios
        .get('/api/brand')
        .then(res => {
          this.brandRd = RemoteData.success<IBrand[], Error>(res.data);
        })
        .catch(e => {
          this.brandRd = RemoteData.failure<IBrand[], Error>(e);
        });
    },
    async add() {
      if ((document.getElementById('new') as HTMLFormElement).reportValidity() === false) {
        return;
      }
      const uuidv1 = require('uuid/v1');
      this.newBrand.id = uuidv1();
      await axios.post('/api/brand', this.newBrand);
      this.newBrand = {} as IBrand;
      this.loadData();
    },
    async update(updateObj: IBrand) {
      if ((document.getElementById(updateObj.id) as HTMLFormElement).reportValidity() === false) {
        return;
      }
      await axios.put('/api/brand', updateObj);
    },
    async remove(id: string) {
      await axios.delete('/api/brand/' + id);
      this.loadData();
    },
    search() {
      axios
        .get('/api/brand/search/?brandName=' + this.searchText)
        .then(res => {
          this.brandRd = RemoteData.success<IBrand[], Error>(res.data);
        })
        .catch(e => {
          this.brandRd = RemoteData.failure<IBrand[], Error>(e);
        });
    }
  }
});
</script>
