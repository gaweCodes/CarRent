<template>
  <div class="container">
    <h1>Reservationen</h1>
    <hr />
    <alert v-if="reservationRd.hasError()">
      <p>{{ reservationRd.getError() }}</p>
    </alert>
    <alert v-if="modelRd.hasError()">
      <p>{{ modelRd.getError() }}</p>
    </alert>
    <alert v-if="carRd.hasError()">
      <p>{{ carRd.getError() }}</p>
    </alert>
    <alert v-if="categoryRd.hasError()">
      <p>{{ categoryRd.getError() }}</p>
    </alert>
    <alert v-if="brandRd.hasError()">
      <p>{{ brandRd.getError() }}</p>
    </alert>
    <loading
      v-if="
        carRd.isLoading() ||
          carRd.isNotAsked() ||
          modelRd.isLoading() ||
          modelRd.isNotAsked() ||
          brandRd.isLoading() ||
          brandRd.isNotAsked() ||
          categoryRd.isLoading() ||
          categoryRd.isNotAsked() ||
          reservationRd.isNotAsked() ||
          reservationRd.isLoading()
      "
    />
    <div
      v-if="
        carRd.hasData() &&
          modelRd.hasData() &&
          brandRd.hasData() &&
          categoryRd.hasData() &&
          reservationRd.hasData()
      "
    >
      <div class="row">
        <div class="col-md-6">
          <div class="form-group">
            <fieldset>
              <legend>Erstellen</legend>
              <input
                type="number"
                id="newDurationInDays"
                v-model="reservation.durationInDays"
                required
                placeholder="Dauer in Tagen"
              />
              <select
                id="newCustomerId"
                required
                v-model="newReservation.customerId"
                title="Kunde wählen"
                class="form-control"
              >
                <option
                  v-for="customer in buildCustomerOptions()"
                  :key="customer.value"
                  :value="customer.value"
                  >{{ customer.text }}</option
                >
              </select>
              <br />
              <select
                id="newCarId"
                required
                v-model="newReservation.carId"
                title="Auto wählen"
                class="form-control"
              >
                <option v-for="car in buildCarOptions()" :key="car.value" :value="car.value">{{
                  car.text
                }}</option>
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
                type="number"
                v-model="searchObject.durationInDays"
                class="form-control"
                placeholder="Nach Dauer suchen"
              />
              <br />
              <select v-model="searchObject.carId" title="Nach Auto suchen" class="form-control">
                <option v-for="car in buildCarOptions()" :key="car.value" :value="car.value">{{
                  car.text
                }}</option>
              </select>
              <br />
              <select
                v-model="searchObject.customerId"
                title="Nach Kunde suchen"
                class="form-control"
              >
                <option
                  v-for="customer in buildCustomerOptions()"
                  :key="customer.value"
                  :value="customer.value"
                  >{{ customer.text }}</option
                >
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
      <!--<fieldset>
        <legend>Übersicht</legend>
        <div class="row">
          <div class="col-md-6">
            <label>Autonummer</label>
          </div>
          <div class="col-md-6">
            <label>Modell</label>
          </div>
        </div>
        <div
          v-for="(car, idx) in carRd.getData()"
          :key="car.id"
          :class="idx % 2 === 0 ? 'row entry even' : 'row entry odd'"
        >
          <div class="col-md-6 form-group">
            <input
              :id="car.id + 'Number'"
              type="text"
              v-model="car.carNumber"
              required
              class="form-control"
              @input="update(car)"
            />
            <button type="button" class="btn btn-danger" @click="remove(carModel.id)">
              <em class="fas fa-trash" />&nbsp;Löschen
            </button>
          </div>
          <div class="col-md-6 form-group">
            <select
              :id="car.id + 'Model'"
              required
              class="form-control"
              v-model="car.carModelid"
              @change="update(car)"
            >
              <option
                v-for="model in buildModelOptions()"
                :key="model.value"
                :value="model.value"
                >{{ model.text }}</option
              >
            </select>
          </div>
        </div>
      </fieldset>-->
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
import { ICustomer } from '@/models/ICustomer';
import { ICar } from '@/models/ICar';
import { IReservation } from '@/models/IReservation';
import { IModelOption } from '@/models/IModelOption';

export default Vue.extend({
  components: { Loading, Alert },
  data() {
    return {
      carRd: RemoteData.notAsked<ICar[], Error>(),
      modelRd: RemoteData.notAsked<ICarModel[], Error>(),
      brandRd: RemoteData.notAsked<IBrand[], Error>(),
      categoryRd: RemoteData.notAsked<ICarCategory[], Error>(),
      reservationRd: RemoteData.notAsked<IReservation[], Error>(),
      newReservation: {} as IReservation,
      searchObject: {}
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
      this.newCar = {} as ICar;
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
      await axios.put('/api/car', updateObj);
    },
    async remove(id: string) {
      await axios.delete('/api/car/' + id);
      this.loadData();
    },
    search() {
      let query = '';
      if (this.searchObject.carNumber && !this.searchObject.carModelId) {
        query = '?carNumber=' + this.searchObject.carNumber;
      } else if (!this.searchObject.carNumber && this.searchObject.carModelId) {
        query = '?carModelId=' + this.searchObject.carModelId;
      } else if (this.searchObject.carNumber && this.searchObject.carModelId) {
        query =
          '?carNumber=' +
          this.searchObject.carNumber +
          '&carModelId=' +
          this.searchObject.carModelId;
      }
      axios
        .get('/api/car/search/' + query)
        .then(res => {
          this.carRd = RemoteData.success<ICar[], Error>(res.data);
        })
        .catch(e => {
          this.carRd = RemoteData.failure<ICar[], Error>(e);
        });
    }
  }
});
</script>
