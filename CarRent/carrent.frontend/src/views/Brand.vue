<template>
  <div class="container">
    <h1>Marken</h1>
    <hr />
    <loading v-if="brandRd.isLoading()" />
    <alert v-if="brandRd.hasError()">
      <p>{{brandRd.getError()}}</p>
    </alert>
    <div class="row">
      <div class="col-md-12">
        <div class="form-group">
          <fieldset>
            <legend>Erstellen</legend>
            <label for="new">Name</label>&nbsp;
            <input type="text" id="new" required v-model="newBrand.title" />
            <br />
            <button type="button" class="btn btn-primary" @click="add">
              <em class="fas fa-plus" /> Erstellen
            </button>
          </fieldset>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col-md-12" v-if="brandRd.hasData()">
        <fieldset>
          <legend>Übersicht</legend>
          <div v-for="brand in brandRd.getData()" :key="brand.id" class="entry">
            <div class="form-group">
              <label :for="brand.id">Name</label>&nbsp;
              <input
                type="text"
                required
                :id="brand.id"
                v-model="brand.title"
                @input="update(brand)"
              />&nbsp;
              <button type="button" class="btn btn-danger" @click="remove(brand.id)">
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
import { IBrand } from '@/models/IBrand';

export default Vue.extend({
  components: { Loading, Alert },
  data() {
    return { brandRd: RemoteData.notAsked<IBrand[], Error>(), newBrand: {} as IBrand };
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
      this.newBrand.title = '';
      this.loadData();
    },
    async update(updateObj: IBrand) {
      if ((document.getElementById(updateObj.id) as HTMLFormElement).reportValidity() === false) {
        return;
      }
      await axios.put('/api/brand/' + updateObj.id, updateObj);
    },
    async remove(id: string) {
      await axios.delete('/api/brand/' + id);
      this.loadData();
    }
  }
});
</script>
