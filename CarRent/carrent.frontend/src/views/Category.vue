<template>
  <div class="container">
    <h1>Kategorien</h1>
    <hr />
    <loading v-if="categoryRd.isLoading()" />
    <alert v-if="categoryRd.hasError()">
      <p>{{categoryRd.getError()}}</p>
    </alert>
    <div class="row">
      <div class="col-md-6">
        <div class="form-group">
          <fieldset>
            <legend>Erstellen</legend>
            <input
              type="text"
              id="newName"
              required
              v-model="newCategory.name"
              placeholder="Name"
              class="form-control"
            />
            <br />
            <input
              type="number"
              id="newFee"
              required
              :min="1"
              v-model="newCategory.dailyFee"
              placeholder="Tagesgebühr"
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
              class="form-control"
              type="text"
              v-model="searchObject.name"
              placeholder="Nach Namen suchen"
            />
            <br />
            <input
              class="form-control"
              type="number"
              v-model="searchObject.fee"
              placeholder="Nach Gebühr suchen"
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
    <fieldset v-if="categoryRd.hasData()">
      <legend>Übersicht</legend>
      <div class="row">
        <div class="col-md-6">
          <label>Name</label>
        </div>
        <div class="col-md-6">
          <label>Gebühr</label>
        </div>
      </div>
      <div
        v-for="(category, idx) in categoryRd.getData()"
        :key="category.id"
        :class="idx % 2 === 0 ? 'row entry even' : 'row entry odd'"
      >
        <div class="col-md-6 form-group">
          <input
            required
            :id="category.id + 'Name'"
            type="text"
            v-model="category.name"
            @input="update(category)"
            class="form-control"
          />
        </div>
        <div class="col-md-6 form-group">
          <input
            required
            :id="category.id + 'Fee'"
            :min="1"
            type="number"
            v-model="category.dailyFee"
            @input="update(category)"
            class="form-control"
          />
        </div>
        <div class="col-md-2 form-group">
          <button type="button" class="btn btn-danger form-control" @click="remove(category.id)">
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
import { ICarCategory } from '@/models/ICarCategory';

export default Vue.extend({
  components: { Loading, Alert },
  data() {
    return {
      categoryRd: RemoteData.notAsked<ICarCategory[], Error>(),
      newCategory: {} as ICarCategory,
      searchObject: { name: undefined, fee: undefined }
    };
  },
  created() {
    this.loadData();
  },
  methods: {
    loadData() {
      axios
        .get('/api/carcategory')
        .then(res => {
          this.categoryRd = RemoteData.success<ICarCategory[], Error>(res.data);
        })
        .catch(e => {
          this.categoryRd = RemoteData.failure<ICarCategory[], Error>(e);
        });
    },
    async add() {
      if (
        (document.getElementById('newName') as HTMLFormElement).reportValidity() === false ||
        (document.getElementById('newFee') as HTMLFormElement).reportValidity() === false
      ) {
        return;
      }
      const uuidv1 = require('uuid/v1');
      this.newCategory.id = uuidv1();
      this.newCategory.dailyFee = Number(this.newCategory.dailyFee);
      await axios.post('/api/carcategory', this.newCategory);
      this.newCategory = {} as ICarCategory;
      this.loadData();
    },
    async update(updateObj: ICarCategory) {
      if (
        (document.getElementById(updateObj.id + 'Name') as HTMLFormElement).reportValidity() ===
          false ||
        (document.getElementById(updateObj.id + 'Fee') as HTMLFormElement).reportValidity() ===
          false
      ) {
        return;
      }
      updateObj.dailyFee = Number(updateObj.dailyFee);
      await axios.put('/api/carcategory', updateObj);
    },
    async remove(id: string) {
      await axios.delete('/api/carcategory/' + id);
      this.loadData();
    },
    search() {
      let query = '';
      if (this.searchObject.name && !this.searchObject.fee) {
        query = '?name=' + this.searchObject.name;
      } else if (!this.searchObject.name && this.searchObject.fee) {
        query = '?fee=' + Number(this.searchObject.fee);
      } else if (this.searchObject.name && this.searchObject.fee) {
        query = '?name=' + this.searchObject.name + '&fee=' + Number(this.searchObject.fee);
      }
      axios
        .get('/api/carcategory/search/' + query)
        .then(res => {
          this.categoryRd = RemoteData.success<ICarCategory[], Error>(res.data);
        })
        .catch(e => {
          this.categoryRd = RemoteData.failure<ICarCategory[], Error>(e);
        });
    }
  }
});
</script>
