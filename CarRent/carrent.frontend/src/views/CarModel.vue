<template>
  <div class="container">
    <h1>Modelle</h1>
    <hr />
    <alert v-if="modelRd.hasError()">
      <p>{{modelRd.getError()}}</p>
    </alert>
    <alert v-if="categoryRd.hasError()">
      <p>{{categoryRd.getError()}}</p>
    </alert>
    <alert v-if="brandRd.hasError()">
      <p>{{brandRd.getError()}}</p>
    </alert>
    <loading
      v-if="modelRd.isLoading() || modelRd.isNotAsked() || brandRd.isLoading() || brandRd.isNotAsked() || categoryRd.isLoading() || categoryRd.isNotAsked()"
    />
    <div v-if="modelRd.hasData() && brandRd.hasData() && categoryRd.hasData()">
      <div class="row">
        <div class="col-md-6">
          <div class="form-group">
            <fieldset>
              <legend>Erstellen</legend>
              <input
                type="text"
                id="newName"
                required
                placeholder="Name"
                v-model="newModel.title"
                class="form-control"
              />
              <br />
              <select
                id="newBrand"
                required
                v-model="newModel.brandId"
                class="form-control"
                title="Marke wählen"
              >
                <option
                  v-for="brand in brandRd.getData()"
                  :key="brand.id"
                  :value="brand.id"
                >{{brand.title}}</option>
              </select>
              <br />
              <select
                id="newCategory"
                required
                v-model="newModel.categoryId"
                title="Kategorie wählen"
                class="form-control"
              >
                <option
                  v-for="category in categoryRd.getData()"
                  :key="category.id"
                  :value="category.id"
                >{{category.name}}</option>
              </select>
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
              <select
                required
                v-model="searchObject.brandId"
                class="form-control"
                title="Marke wählen"
              >
                <option
                  v-for="brand in brandRd.getData()"
                  :key="brand.id"
                  :value="brand.id"
                >{{brand.title}}</option>
              </select>
              <br />
              <select
                required
                v-model="searchObject.categoryId"
                title="Kategorie wählen"
                class="form-control"
              >
                <option
                  v-for="category in categoryRd.getData()"
                  :key="category.id"
                  :value="category.id"
                >{{category.name}}</option>
              </select>
              <br />
              <button type="button" class="btn btn-primary" @click="search()">
                <em class="fas fa-search" />&nbsp;Suchen
              </button>
            </fieldset>
          </div>
        </div>
      </div>
      <br />
      <fieldset>
        <legend>Übersicht</legend>
        <div class="row">
          <div class="col-md-4">
            <label>Name</label>
          </div>
          <div class="col-md-4">
            <label>Marke</label>
          </div>
          <div class="col-md-4">
            <label>Kategorie</label>
          </div>
        </div>
        <div
          v-for="(carModel, idx) in modelRd.getData()"
          :key="carModel.id"
          :class="idx % 2 === 0 ? 'row entry even' : 'row entry odd'"
        >
          <div class="col-md-4 form-group">
            <input
              :id="carModel.id + 'Name'"
              type="text"
              v-model="carModel.title"
              required
              @input="update(carModel)"
              class="form-control"
            />
            <button type="button" class="btn btn-danger" @click="remove(carModel.id)">
              <em class="fas fa-trash" />&nbsp;Löschen
            </button>
          </div>
          <div class="col-md-4 form-group">
            <select
              :id="carModel.id + 'Brand'"
              required
              v-model="carModel.brandId"
              class="form-control"
              @change="update(carModel)"
            >
              <option
                v-for="brand in brandRd.getData()"
                :key="'Update' + brand.id"
                :value="brand.id"
              >{{brand.title}}</option>
            </select>&nbsp;
          </div>
          <div class="col-md-4 form-group">
            <select
              class="form-control"
              :id="carModel.id + 'Category'"
              required
              v-model="carModel.categoryId"
              @change="update(carModel)"
            >
              <option
                v-for="category in categoryRd.getData()"
                :key="'Update' + category.id"
                :value="category.id"
              >{{category.name}}</option>
            </select>
          </div>
        </div>
      </fieldset>
    </div>
  </div>
</template>
<script lang="ts">
import Vue from 'vue';
import axios from 'axios';
import { RemoteData } from '@/helpers/RemoteData';
import Loading from '@/components/Loading.vue';
import Alert from '@/components/Alert.vue';
import { ICarModel } from '@/models/ICarModel';
import { ICarCategory } from '@/models/ICarCategory';
import { IBrand } from '@/models/IBrand';

export default Vue.extend({
  components: { Loading, Alert },
  data() {
    return {
      modelRd: RemoteData.notAsked<ICarModel[], Error>(),
      brandRd: RemoteData.notAsked<IBrand[], Error>(),
      categoryRd: RemoteData.notAsked<ICarCategory[], Error>(),
      newModel: {} as ICarModel,
      searchObject: { name: undefined, brandId: undefined, categoryId: undefined }
    };
  },
  created() {
    this.loadData();
    this.loadBrands();
    this.loadCategories();
  },
  methods: {
    loadCategories() {
      axios
        .get('/api/carcategory')
        .then(res => {
          this.categoryRd = RemoteData.success<ICarCategory[], Error>(res.data);
        })
        .catch(e => {
          this.categoryRd = RemoteData.failure<ICarCategory[], Error>(e);
        });
    },
    loadBrands() {
      axios
        .get('/api/brand')
        .then(res => {
          this.brandRd = RemoteData.success<IBrand[], Error>(res.data);
        })
        .catch(e => {
          this.brandRd = RemoteData.failure<IBrand[], Error>(e);
        });
    },
    loadData() {
      axios
        .get('/api/carModel')
        .then(res => {
          this.modelRd = RemoteData.success<ICarModel[], Error>(res.data);
        })
        .catch(e => {
          this.modelRd = RemoteData.failure<ICarModel[], Error>(e);
        });
    },
    async add() {
      if (
        (document.getElementById('newName') as HTMLFormElement).reportValidity() === false ||
        (document.getElementById('newBrand') as HTMLFormElement).reportValidity() === false ||
        (document.getElementById('newCategory') as HTMLFormElement).reportValidity() === false
      ) {
        return;
      }
      const uuidv1 = require('uuid/v1');
      this.newModel.id = uuidv1();
      await axios.post('/api/carModel', this.newModel);
      this.newModel = {} as ICarModel;
      this.loadData();
    },
    async update(updateObj: ICarModel) {
      if (
        (document.getElementById(updateObj.id + 'Name') as HTMLFormElement).reportValidity() ===
          false ||
        (document.getElementById(updateObj.id + 'Brand') as HTMLFormElement).reportValidity() ===
          false ||
        (document.getElementById(updateObj.id + 'Category') as HTMLFormElement).reportValidity() ===
          false
      ) {
        return;
      }
      await axios.put('/api/carModel', updateObj);
    },
    async remove(id: string) {
      await axios.delete('/api/carModel/' + id);
      this.loadData();
    },
    search() {
      let query = '';
      if (this.searchObject.name && !this.searchObject.brandId && !this.searchObject.categoryId) {
        query = '?name=' + this.searchObject.name;
      } else if (
        !this.searchObject.name &&
        this.searchObject.brandId &&
        !this.searchObject.categoryId
      ) {
        query = '?brandId=' + this.searchObject.brandId;
      } else if (
        !this.searchObject.name &&
        !this.searchObject.brandId &&
        this.searchObject.categoryId
      ) {
        query = '?categoryId=' + this.searchObject.categoryId;
      } else if (
        this.searchObject.name &&
        this.searchObject.brandId &&
        !this.searchObject.categoryId
      ) {
        query = '?name=' + this.searchObject.name + '&brandId=' + this.searchObject.brandId;
      } else if (
        this.searchObject.name &&
        !this.searchObject.brandId &&
        this.searchObject.categoryId
      ) {
        query = '?name=' + this.searchObject.name + '&categoryId' + this.searchObject.categoryId;
      } else if (
        !this.searchObject.name &&
        this.searchObject.brandId &&
        this.searchObject.categoryId
      ) {
        query =
          '?brandId=' + this.searchObject.brandId + '&categoryId' + this.searchObject.categoryId;
      } else if (
        this.searchObject.name &&
        this.searchObject.brandId &&
        this.searchObject.categoryId
      ) {
        query =
          '?name=' +
          this.searchObject.name +
          '&brandId=' +
          this.searchObject.brandId +
          '&categoryId' +
          this.searchObject.categoryId;
      }
      axios
        .get('/api/carmodel/search/' + query)
        .then(res => {
          this.modelRd = RemoteData.success<ICarModel[], Error>(res.data);
        })
        .catch(e => {
          this.modelRd = RemoteData.failure<ICarModel[], Error>(e);
        });
    }
  }
});
</script>
