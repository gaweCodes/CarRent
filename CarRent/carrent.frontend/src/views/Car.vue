<template>
  <div class="container">
    <h1>Autos</h1>
    <hr />
    <alert v-if="modelRd.hasError()">
      <p>{{modelRd.getError()}}</p>
    </alert>
    <alert v-if="carRd.hasError()">
      <p>{{carRd.getError()}}</p>
    </alert>
    <alert v-if="categoryRd.hasError()">
      <p>{{categoryRd.getError()}}</p>
    </alert>
    <alert v-if="brandRd.hasError()">
      <p>{{brandRd.getError()}}</p>
    </alert>
    <loading
      v-if="carRd.isLoading() || carRd.isNotAsked() || modelRd.isLoading() || modelRd.isNotAsked() || brandRd.isLoading() || brandRd.isNotAsked() || categoryRd.isLoading() || categoryRd.isNotAsked()"
    />
    <div v-if="carRd.hasData() && modelRd.hasData() && brandRd.hasData() && categoryRd.hasData()">
      <div class="row">
        <div class="col-md-12">
          <div class="form-group">
            <fieldset>
              <legend>Erstellen</legend>
              <label for="newNumber">Autonummer</label>&nbsp;
              <input type="text" id="newNumber" required v-model="newCar.carNumber" />&nbsp;
              <label for="newModel">Modell</label>&nbsp;
              <select id="newModel" required v-model="newCar.carModelId">
                <option
                  v-for="model in buildModelOptions()"
                  :key="model.value"
                  :value="model.value"
                >{{model.text}}</option>
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
            <div v-for="(car, idx) in carRd.getData()" :key="car.id" class="entry">
              <div
                class="form-group"
                :style="idx % 2 === 0 ? 'background-color: white;' : 'background-color: lightgray;' "
              >
                <label :for="car.id + 'Number'">Autonummer</label>&nbsp;
                <input
                  :id="car.id + 'Number'"
                  type="text"
                  v-model="car.carNumber"
                  required
                  @input="update(car)"
                />&nbsp;
                <label :for="car.id + 'Model'">Modell</label>
                &nbsp;
                <select
                  :id="car.id + 'Model'"
                  required
                  v-model="car.carModelid"
                  @change="update(car)"
                >
                  <option
                    v-for="model in buildModelOptions()"
                    :key="model.value"
                    :value="model.value"
                  >{{model.text}}</option>
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
import { ICar } from '@/models/ICar';
import { IModelOption } from '@/models/IModelOption';

export default Vue.extend({
  components: { Loading, Alert },
  data() {
    return {
      carRd: RemoteData.notAsked<ICar[], Error>(),
      modelRd: RemoteData.notAsked<ICarModel[], Error>(),
      brandRd: RemoteData.notAsked<IBrand[], Error>(),
      categoryRd: RemoteData.notAsked<ICarCategory[], Error>(),
      newCar: {} as ICar
    };
  },
  async created() {
    await this.loadData();
  },
  methods: {
    buildModelOptions(): IModelOption[] {
      const modelOptions = [] as IModelOption[];
      if (this.modelRd.hasData()) {
        this.modelRd.getData().forEach((model: ICarModel) => {
          const brand = this.brandRd.getData().find(x => x.id === model.brandId);
          if (brand === undefined) {
            return;
          }
          const category = this.categoryRd.getData().find(x => x.id === model.categoryId);
          if (category === undefined) {
            return;
          }
          modelOptions.push({
            value: model.id,
            text:
              brand.title +
              ' - ' +
              model.title +
              ' - ' +
              category.name +
              ' - CHF ' +
              category.dailyFee +
              '/Tag'
          } as IModelOption);
        });
      }
      return modelOptions;
    },
    async loadData() {
      await this.loadModelData();
      await this.loadBrands();
      await this.loadCategories();
      await axios
        .get('/api/car')
        .then(res => {
          this.carRd = RemoteData.success<ICar[], Error>(res.data);
        })
        .catch(e => {
          this.carRd = RemoteData.failure<ICar[], Error>(e);
        });
    },
    async loadCategories() {
      axios
        .get('/api/carcategory')
        .then(res => {
          this.categoryRd = RemoteData.success<ICarCategory[], Error>(res.data);
        })
        .catch(e => {
          this.categoryRd = RemoteData.failure<ICarCategory[], Error>(e);
        });
    },
    async loadBrands() {
      axios
        .get('/api/brand')
        .then(res => {
          this.brandRd = RemoteData.success<IBrand[], Error>(res.data);
        })
        .catch(e => {
          this.brandRd = RemoteData.failure<IBrand[], Error>(e);
        });
    },
    async loadModelData() {
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
        (document.getElementById('newNumber') as HTMLFormElement).reportValidity() === false ||
        (document.getElementById('newModel') as HTMLFormElement).reportValidity() === false
      ) {
        return;
      }
      const uuidv1 = require('uuid/v1');
      this.newCar.id = uuidv1();
      await axios.post('/api/car', this.newCar);
      this.newCar.carNumber = '';
      this.newCar.carModelId = '';
      this.loadData();
    },
    async update(updateObj: ICarModel) {
      if (
        (document.getElementById(updateObj.id + 'Number') as HTMLFormElement).reportValidity() ===
          false ||
        (document.getElementById(updateObj.id + 'Model') as HTMLFormElement).reportValidity() ===
          false
      ) {
        return;
      }
      await axios.put('/api/car/' + updateObj.id, updateObj);
    },
    async remove(id: string) {
      await axios.delete('/api/car/' + id);
      this.loadData();
    }
  }
});
</script>
