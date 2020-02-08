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
    <div v-else-if="modelRd.hasData() || brandRd.hasData() || categoryRd.hasData()">
      <div class="row">
        <div class="col-md-12">
          <div class="form-group">
            <fieldset>
              <legend>Erstellen</legend>
              <label for="newName">Name</label>&nbsp;
              <input type="text" id="newName" required v-model="newModel.title" />&nbsp;
              <label for="newBrand">Marke</label>&nbsp;
              <select id="newBrand" required v-model="newModel.brandId">
                <option
                  v-for="brand in brandRd.getData()"
                  :key="brand.id"
                  :value="brand.id"
                >{{brand.title}}</option>
              </select>&nbsp;
              <label for="newCategory">Kategorie</label>&nbsp;
              <select id="newCategory" required v-model="newModel.categoryId">
                <option
                  v-for="category in categoryRd.getData()"
                  :key="category.id"
                  :value="category.id"
                >{{category.name}}</option>
              </select>
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
        <div class="col-md-12">
          <fieldset>
            <legend>Übersicht</legend>
            <div v-for="carModel in modelRd.getData()" :key="carModel.id" class="entry">
              <div class="form-group">
                <label :for="carModel.id + 'Name'">Name</label>&nbsp;
                <input
                  :id="carModel.id + 'Name'"
                  type="text"
                  v-model="carModel.title"
                  required
                  @input="update(carModel)"
                />&nbsp;
                <label :for="carModel.id + 'Category'">Marke</label>&nbsp;
                <select
                  :id="carModel.id + 'Brand'"
                  required
                  v-model="carModel.brandId"
                  @change="update(carModel)"
                >
                  <option
                    v-for="brand in brandRd.getData()"
                    :key="'Update' + brand.id"
                    :value="brand.id"
                  >{{brand.title}}</option>
                </select>&nbsp;
                <label :for="carModel.id + 'Category'">Kategorie</label>&nbsp;
                <select
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
                <button type="button" class="btn btn-danger" @click="remove(carModel.id)">
                  <em class="fas fa-trash" />
                </button>
              </div>
            </div>
          </fieldset>
        </div>
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
import { ICarModel } from '@/models/ICarModel';
import { ICarCategory } from '@/models/ICarCategory';
import { IBrand } from '@/models/IBrand';

export default Vue.extend({
  components: { Loading, Alert },
  data() {
    return {
      modelRd: RemoteData.notAsked<ICarModel, Error>(),
      brandRd: RemoteData.notAsked<IBrand, Error>(),
      categoryRd: RemoteData.notAsked<ICarCategory, Error>(),
      newModel: {} as ICarModel
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
          this.categoryRd = RemoteData.success<ICarCategory, Error>(res.data);
        })
        .catch(e => {
          this.categoryRd = RemoteData.failure<ICarCategory, Error>(e);
        });
    },
    loadBrands() {
      axios
        .get('/api/brand')
        .then(res => {
          this.brandRd = RemoteData.success<IBrand, Error>(res.data);
        })
        .catch(e => {
          this.brandRd = RemoteData.failure<IBrand, Error>(e);
        });
    },
    loadData() {
      axios
        .get('/api/carModel')
        .then(res => {
          this.modelRd = RemoteData.success<ICarModel, Error>(res.data);
        })
        .catch(e => {
          this.modelRd = RemoteData.failure<ICarModel, Error>(e);
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
      this.newModel.title = '';
      this.newModel.brandId = '';
      this.newModel.categoryId = '';
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
      await axios.put('/api/carModel/' + updateObj.id, updateObj);
    },
    async remove(id: string) {
      await axios.delete('/api/carModel/' + id);
      this.loadData();
    }
  }
});
</script>
