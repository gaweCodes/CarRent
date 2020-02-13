<template>
  <div class="container">
    <h1>Reservationen / Verträge</h1>
    <hr />
    <alert v-if="reservationRd.hasError()">
      <p>{{ reservationRd.getError() }}</p>
    </alert>
    <alert v-if="customerRd.hasError()">
      <p>{{ customerRd.getError() }}</p>
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
      v-if="carRd.isLoading() || carRd.isNotAsked() || modelRd.isLoading() || modelRd.isNotAsked() || brandRd.isLoading() || brandRd.isNotAsked() || categoryRd.isLoading() || categoryRd.isNotAsked() || reservationRd.isNotAsked() || reservationRd.isLoading() || customerRd.isNotAsked() || customerRd.isLoading()"
    />
    <div
      v-else-if="carRd.hasData() &&
          modelRd.hasData() &&
          brandRd.hasData() &&
          categoryRd.hasData() &&
          reservationRd.hasData() &&
          customerRd.hasData()"
    >
      <div class="row">
        <div class="col-md-6">
          <div class="form-group">
            <fieldset>
              <legend>Erstellen</legend>
              <input
                type="number"
                id="newDurationInDays"
                v-model="newReservation.durationInDays"
                required
                :min="1"
                class="form-control"
                placeholder="Dauer in Tagen"
              />
              <br />
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
                >{{ customer.text }}</option>
              </select>
              <br />
              <select v-model="selectedCategoryId" title="Kategorie wählen" class="form-control">
                <option
                  v-for="category in categoryRd.getData()"
                  :key="category.id"
                  :value="category.id"
                >{{category.name}} - CHF {{category.dailyFee}} pro Tag</option>
              </select>
              <br />
              <select
                id="newCarId"
                required
                v-model="newReservation.carId"
                title="Auto wählen"
                class="form-control"
              >
                <option
                  v-for="car in buildCarOptions"
                  :key="car.value"
                  :value="car.value"
                >{{car.text}}</option>
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
              <select
                v-model="searchObject.customerId"
                title="Nach Kunde suchen"
                class="form-control"
              >
                <option
                  v-for="customer in buildCustomerOptions()"
                  :key="customer.value"
                  :value="customer.value"
                >{{ customer.text }}</option>
              </select>
              <br />
              <select
                v-model="selectedSearchCategoryId"
                title="Kategorie suchen"
                class="form-control"
              >
                <option
                  v-for="category in categoryRd.getData()"
                  :key="category.id"
                  :value="category.id"
                >{{category.name}} - CHF {{category.dailyFee}} pro Tag</option>
              </select>
              <br />
              <select v-model="searchObject.carId" title="Nach Auto suchen" class="form-control">
                <option
                  v-for="car in buildCarOptionsSearch"
                  :key="car.value"
                  :value="car.value"
                >{{car.text}}</option>
              </select>
              <br />
              <input
                type="number"
                v-model="searchObject.totalCost"
                class="form-control"
                placeholder="Nach Kosten suchen"
              />
              <br />
              <select v-model="searchObject.state" title="Nach Status suchen" class="form-control">
                <option value="Active">Reservation</option>
                <option value="Closed">Vertrag</option>
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
          <div class="col-md-2">
            <label>Dauer</label>
          </div>
          <div class="col-md-2">
            <label>Kosten</label>
          </div>
          <div class="col-md-8">
            <label>Kunde</label>
          </div>
          <div class="col-md-10">
            <label>Auto</label>
          </div>
          <div class="col-md-2">
            <label>Status</label>
          </div>
        </div>
        <div
          v-for="(reservation, idx) in reservationRd.getData()"
          :key="reservation.id"
          :class="idx % 2 === 0 ? 'row entry even' : 'row entry odd'"
        >
          <div class="col-md-2 form-group">
            <input
              :id="reservation.id + 'DurationInDays'"
              type="number"
              v-model="reservation.durationInDays"
              required
              :readonly="reservation.state === 'Closed'"
              :min="1"
              class="form-control"
              @input="update(reservation)"
            />
          </div>
          <div class="col-md-2 form-group">
            <input
              :id="reservation.id + 'TotalCost'"
              type="number"
              v-model="reservation.totalCost"
              readonly
              class="form-control"
            />
          </div>
          <div class="col-md-8 form-group">
            <select
              :id="reservation.id + 'CustomerId'"
              required
              :disabled="reservation.state === 'Closed'"
              v-model="reservation.customerId"
              title="Kunde wählen"
              class="form-control"
              @change="update(reservation)"
            >
              <option
                v-for="customer in buildCustomerOptions()"
                :key="customer.value"
                :value="customer.value"
              >{{ customer.text }}</option>
            </select>
          </div>
          <div class="col-md-10 form-group">
            <select
              :id="reservation.id + 'CarId'"
              required
              :disabled="reservation.state === 'Closed'"
              v-model="reservation.carId"
              title="Auto wählen"
              class="form-control"
              @change="update(reservation)"
            >
              <option
                v-for="car in buildChangeCarOptions"
                :key="car.value"
                :value="car.value"
              >{{car.text}}</option>
            </select>
          </div>
          <div class="col-md-2 form-group">
            <input
              :id="reservation.id + 'State'"
              type="text"
              v-model="reservation.state"
              readonly
              class="form-control"
            />
          </div>
          <div class="col-md-2 form-group">
            <button type="button" class="btn btn-primary form-control" @click="pickUp(reservation)">
              <em class="fas fa-truck" />&nbsp;Abholung
            </button>
          </div>
          <div class="col-md-2 form-group">
            <button
              type="button"
              class="btn btn-danger form-control"
              @click="remove(reservation.id)"
            >
              <em class="fas fa-trash" />&nbsp;Löschen
            </button>
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
import { ICustomer } from '@/models/ICustomer';
import { ICar } from '@/models/ICar';
import { IDropdownOption } from '@/models/IDropdownOption';
import { IReservation } from '../models/IReservation';

export default Vue.extend({
  components: { Loading, Alert },
  data() {
    return {
      carRd: RemoteData.notAsked<ICar[], Error>(),
      modelRd: RemoteData.notAsked<ICarModel[], Error>(),
      customerRd: RemoteData.notAsked<ICustomer[], Error>(),
      brandRd: RemoteData.notAsked<IBrand[], Error>(),
      categoryRd: RemoteData.notAsked<ICarCategory[], Error>(),
      reservationRd: RemoteData.notAsked<IReservation[], Error>(),
      newReservation: {} as IReservation,
      selectedCategoryId: '',
      selectedSearchCategoryId: '',
      searchObject: {
        carId: undefined,
        customerId: undefined,
        totalCost: undefined,
        durationInDays: undefined,
        state: undefined
      }
    };
  },
  created() {
    this.loadData();
  },
  computed: {
    buildChangeCarOptions(): IDropdownOption[] {
      const carOptions = [] as IDropdownOption[];
      this.carRd.getData().forEach((car: ICar) => {
        // @ts-ignore
        const model = this.modelRd.getData().find(m => m.id === car.carModelid);
        if (model === undefined) {
          return;
        }
        const brand = this.brandRd.getData().find(x => x.id === model.brandId);
        if (brand === undefined) {
          return;
        }
        const category = this.categoryRd.getData().find(c => c.id === model.categoryId);
        if (category === undefined) {
          return;
        }
        carOptions.push({
          value: car.id,
          text:
            car.carNumber +
            ' - ' +
            brand.title +
            ' - ' +
            model.title +
            ' - ' +
            category.name +
            ' für CHF ' +
            category.dailyFee +
            ' / Tag'
        } as IDropdownOption);
      });
      return carOptions;
    },
    buildCarOptionsSearch(): IDropdownOption[] {
      const carOptions = [] as IDropdownOption[];
      const models = this.modelRd
        .getData()
        .filter(x => x.categoryId === this.selectedSearchCategoryId);
      if (models.length === 0) {
        return carOptions;
      }

      const cars = [] as ICar[];
      models.forEach((model: ICarModel) => {
        cars.push(
          ...this.carRd.getData().filter(c => {
            // @ts-ignore
            return c.carModelid === model.id;
          })
        );
      });
      cars.forEach((car: ICar) => {
        // @ts-ignore
        const model = models.find(m => m.id === car.carModelid);
        if (model === undefined) {
          return;
        }
        const brand = this.brandRd.getData().find(x => x.id === model.brandId);
        if (brand === undefined) {
          return;
        }
        carOptions.push({
          value: car.id,
          text: car.carNumber + ' - ' + brand.title + ' - ' + model.title
        } as IDropdownOption);
      });
      return carOptions;
    },
    buildCarOptions(): IDropdownOption[] {
      const carOptions = [] as IDropdownOption[];
      const models = this.modelRd.getData().filter(x => x.categoryId === this.selectedCategoryId);
      if (models.length === 0) {
        return carOptions;
      }

      const cars = [] as ICar[];
      models.forEach((model: ICarModel) => {
        cars.push(
          ...this.carRd.getData().filter(c => {
            // @ts-ignore
            return c.carModelid === model.id;
          })
        );
      });
      cars.forEach((car: ICar) => {
        // @ts-ignore
        const model = models.find(m => m.id === car.carModelid);
        if (model === undefined) {
          return;
        }
        const brand = this.brandRd.getData().find(x => x.id === model.brandId);
        if (brand === undefined) {
          return;
        }
        carOptions.push({
          value: car.id,
          text: car.carNumber + ' - ' + brand.title + ' - ' + model.title
        } as IDropdownOption);
      });
      return carOptions;
    }
  },
  methods: {
    buildCustomerOptions(): IDropdownOption[] {
      const customerOptions = [] as IDropdownOption[];
      if (this.customerRd.hasData()) {
        this.customerRd.getData().forEach((customer: ICustomer) => {
          customerOptions.push({
            value: customer.id,
            text: customer.firstName + ' ' + customer.lastName + ', ' + customer.address
          } as IDropdownOption);
        });
      }
      return customerOptions;
    },
    loadData() {
      this.loadBrands().then(() => {
        this.loadCategories().then(() => {
          this.loadModelData().then(() => {
            this.loadCarData().then(() => {
              this.loadCustomerData().then(() => {
                axios
                  .get('/api/reservation')
                  .then(res => {
                    this.reservationRd = RemoteData.success<IReservation[], Error>(res.data);
                    if (this.categoryRd.getData().length > 0) {
                      this.selectedCategoryId = this.categoryRd.getData()[0].id;
                    }
                  })
                  .catch(e => {
                    this.reservationRd = RemoteData.failure<IReservation[], Error>(e);
                  });
              });
            });
          });
        });
      });
    },
    async loadCustomerData() {
      await axios
        .get('/api/customer')
        .then(res => {
          this.customerRd = RemoteData.success<ICustomer[], Error>(res.data);
        })
        .catch(e => {
          this.customerRd = RemoteData.failure<ICustomer[], Error>(e);
        });
    },
    async loadCarData() {
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
        (document.getElementById('newDurationInDays') as HTMLFormElement).reportValidity() ===
          false ||
        (document.getElementById('newCustomerId') as HTMLFormElement).reportValidity() === false ||
        (document.getElementById('newCarId') as HTMLFormElement).reportValidity() === false
      ) {
        return;
      }
      const uuidv1 = require('uuid/v1');
      this.newReservation.id = uuidv1();
      this.newReservation.durationInDays = Number(this.newReservation.durationInDays);
      await axios.post('/api/reservation', this.newReservation);
      this.newReservation = {} as IReservation;
      this.loadData();
    },
    async update(updateObj: IReservation) {
      if (
        (document.getElementById(
          updateObj.id + 'DurationInDays'
        ) as HTMLFormElement).reportValidity() === false ||
        (document.getElementById(
          updateObj.id + 'CustomerId'
        ) as HTMLFormElement).reportValidity() === false ||
        (document.getElementById(updateObj.id + 'CarId') as HTMLFormElement).reportValidity() ===
          false
      ) {
        return;
      }
      updateObj.durationInDays = Number(updateObj.durationInDays);
      await axios.put('/api/reservation', updateObj);
      this.loadData();
    },
    async remove(id: string) {
      await axios.delete('/api/reservation/' + id);
      this.loadData();
    },
    async pickUp(reservation: IReservation) {
      await axios.post('/api/reservation/pickup', reservation);
      this.loadData();
    },
    search() {
      let query = '';
      if (
        this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        !this.searchObject.carId &&
        !this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query = '?durationInDays=' + Number(this.searchObject.durationInDays);
      } else if (
        !this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        !this.searchObject.carId &&
        !this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query = '?customerId=' + this.searchObject.customerId;
      } else if (
        !this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        this.searchObject.carId &&
        !this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query = '?carId=' + this.searchObject.carId;
      } else if (
        !this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        !this.searchObject.carId &&
        this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query = '?totalCost=' + Number(this.searchObject.totalCost);
      } else if (
        !this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        !this.searchObject.carId &&
        !this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query = '?state=' + this.searchObject.state;
      } else if (
        this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        !this.searchObject.carId &&
        !this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query =
          '?curationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&customerId=' +
          this.searchObject.customerId;
      } else if (
        this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        this.searchObject.carId &&
        !this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query =
          '?curationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&carId=' +
          this.searchObject.carId;
      } else if (
        this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        !this.searchObject.carId &&
        this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query =
          '?curationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&totalCost=' +
          Number(this.searchObject.totalCost);
      } else if (
        this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        !this.searchObject.carId &&
        !this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query =
          '?curationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&state=' +
          this.searchObject.state;
      } else if (
        !this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        this.searchObject.carId &&
        !this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query = '?customerId=' + this.searchObject.customerId + '&carId=' + this.searchObject.carId;
      } else if (
        !this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        !this.searchObject.carId &&
        this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query =
          '?customerId=' +
          this.searchObject.customerId +
          '&totalCost=' +
          Number(this.searchObject.totalCost);
      } else if (
        this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        !this.searchObject.carId &&
        !this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query = '?customerid=' + this.searchObject.customerId + '&state=' + this.searchObject.state;
      } else if (
        !this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        this.searchObject.carId &&
        this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query =
          '?carId=' + this.searchObject.carId + '&totalCost=' + Number(this.searchObject.totalCost);
      } else if (
        !this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        this.searchObject.carId &&
        !this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query = '?carId=' + this.searchObject.carId + '&state=' + this.searchObject.state;
      } else if (
        !this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        !this.searchObject.carId &&
        this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query =
          '?totalCost=' + Number(this.searchObject.totalCost) + '&state=' + this.searchObject.state;
      } else if (
        this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        this.searchObject.carId &&
        !this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query =
          '?curationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&customerId=' +
          this.searchObject.customerId +
          '&carId=' +
          this.searchObject.carId;
      } else if (
        this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        this.searchObject.carId &&
        this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query =
          '?curationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&carId=' +
          this.searchObject.carId +
          '&totalCost=' +
          Number(this.searchObject.totalCost);
      } else if (
        this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        !this.searchObject.carId &&
        this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query =
          '?curationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&customerId=' +
          this.searchObject.customerId +
          '&totalCost=' +
          Number(this.searchObject.totalCost);
      } else if (
        !this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        this.searchObject.carId &&
        this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query =
          '?customerId=' +
          this.searchObject.customerId +
          '&carId=' +
          this.searchObject.carId +
          '&totalCost=' +
          Number(this.searchObject.totalCost);
      } else if (
        !this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        this.searchObject.carId &&
        this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query =
          '?carid=' +
          this.searchObject.carId +
          '&totalCost=' +
          Number(this.searchObject.totalCost) +
          '&state=' +
          this.searchObject.state;
      } else if (
        !this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        !this.searchObject.carId &&
        this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query =
          '?customerId=' +
          this.searchObject.customerId +
          '&totalCost=' +
          Number(this.searchObject.totalCost) +
          '&state=' +
          this.searchObject.state;
      } else if (
        !this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        this.searchObject.carId &&
        !this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query =
          '?customerId=' +
          this.searchObject.customerId +
          '&carId=' +
          this.searchObject.carId +
          '&state=' +
          this.searchObject.state;
      } else if (
        this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        !this.searchObject.carId &&
        this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query =
          '?durationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&totalCost=' +
          Number(this.searchObject.totalCost) +
          '&state=' +
          this.searchObject.state;
      } else if (
        this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        this.searchObject.carId &&
        !this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query =
          '?durationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&carId=' +
          this.searchObject.carId +
          '&state=' +
          this.searchObject.state;
      } else if (
        this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        !this.searchObject.carId &&
        !this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query =
          '?durationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&customerId=' +
          this.searchObject.customerId +
          '&state=' +
          this.searchObject.state;
      } else if (
        this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        !this.searchObject.carId &&
        this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query =
          '?customerId=' +
          this.searchObject.customerId +
          '&durationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&totalCost=' +
          Number(this.searchObject.totalCost);
      } else if (
        !this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        this.searchObject.carId &&
        this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query =
          '?customerId=' +
          this.searchObject.customerId +
          '&carId=' +
          this.searchObject.carId +
          '&totalCost=' +
          Number(this.searchObject.totalCost) +
          '&state=' +
          this.searchObject.state;
      } else if (
        this.searchObject.durationInDays &&
        !this.searchObject.customerId &&
        this.searchObject.carId &&
        this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query =
          '?durationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&carId=' +
          this.searchObject.carId +
          '&totalCost=' +
          Number(this.searchObject.totalCost) +
          '&state=' +
          this.searchObject.state;
      } else if (
        this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        !this.searchObject.carId &&
        this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query =
          '?durationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&customerId=' +
          this.searchObject.customerId +
          '&totalCost=' +
          Number(this.searchObject.totalCost) +
          '&state=' +
          this.searchObject.state;
      } else if (
        this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        this.searchObject.carId &&
        !this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query =
          '?durationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&carId=' +
          this.searchObject.carId +
          '&customerId=' +
          this.searchObject.customerId +
          '&state=' +
          this.searchObject.state;
      } else if (
        this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        this.searchObject.carId &&
        this.searchObject.totalCost &&
        !this.searchObject.state
      ) {
        query =
          '?durationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&carId=' +
          this.searchObject.carId +
          '&totalCost=' +
          Number(this.searchObject.totalCost) +
          '&customerId=' +
          this.searchObject.customerId;
      } else if (
        this.searchObject.durationInDays &&
        this.searchObject.customerId &&
        this.searchObject.carId &&
        this.searchObject.totalCost &&
        this.searchObject.state
      ) {
        query =
          '?durationInDays=' +
          Number(this.searchObject.durationInDays) +
          '&carId=' +
          this.searchObject.carId +
          '&totalCost=' +
          Number(this.searchObject.totalCost) +
          '&state=' +
          this.searchObject.state +
          '&customerId=' +
          this.searchObject.customerId;
      }
      axios
        .get('/api/reservation/search/' + query)
        .then(res => {
          this.reservationRd = RemoteData.success<IReservation[], Error>(res.data);
        })
        .catch(e => {
          this.reservationRd = RemoteData.failure<IReservation[], Error>(e);
        });
    }
  }
});
</script>
