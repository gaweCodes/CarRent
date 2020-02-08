<template>
  <div class="container">
    <h1>Kategorien</h1>
    <hr />
    <loading v-if="categoryRd.isLoading()" />
    <alert v-if="categoryRd.hasError()">
      <p>{{categoryRd.getError()}}</p>
    </alert>
    <div class="row">
      <div class="col-md-12">
        <div class="form-group">
          <fieldset>
            <legend>Erstellen</legend>
            <label for="newName">Name</label>&nbsp;
            <input type="text" id="newName" required v-model="newCategory.name" />&nbsp;
            <label for="newFee">Tages Gebühr</label>&nbsp;
            <input type="number" id="newFee" required :min="1" v-model="newCategory.dailyFee" />
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
      <div class="col-md-12" v-if="categoryRd.hasData()">
        <fieldset>
          <legend>Übersicht</legend>
          <div v-for="category in categoryRd.getData()" :key="category.id" class="entry">
            <div class="form-group">
              <label :for="category.id + 'Name'">Name</label>&nbsp;
              <input
                :id="category.id + 'Name'"
                required
                type="text"
                v-model="category.name"
                @input="update(category)"
              />&nbsp;
              <label :for="category.id + 'Fee'">Tägliche Gebühr</label>&nbsp;
              <input
                required
                :min="1"
                :id="category.id + 'Fee'"
                type="number"
                v-model="category.dailyFee"
                @input="update(category)"
              />&nbsp;
              <button type="button" class="btn btn-danger" @click="remove(category.id)">
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
import { ICarCategory } from '@/models/ICarCategory';

export default Vue.extend({
  components: { Loading, Alert },
  data() {
    return {
      categoryRd: RemoteData.notAsked<ICarCategory[], Error>(),
      newCategory: {} as ICarCategory
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
      this.newCategory.name = '';
      this.newCategory.dailyFee = 0.0;
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
      await axios.put('/api/carcategory/' + updateObj.id, updateObj);
    },
    async remove(id: string) {
      await axios.delete('/api/carcategory/' + id);
      this.loadData();
    }
  }
});
</script>
