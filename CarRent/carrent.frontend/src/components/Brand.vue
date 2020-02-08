<template>
  <div class="container">
    <h1 v-if="brandRd.hasData()">{{ brandRd.getData()[0].title }}</h1>
  </div>
</template>
<script lang="ts">
import Vue from 'vue';
import axios from 'axios';
import { RemoteData } from '@/helpers/RemoteData';

export default Vue.extend({
  data() {
    return { brandRd: RemoteData.notAsked<any, Error>() };
  },
  created() {
    // const uuidv1 = require('uuid/v1');
    // axios.post('/api/brand', { id: uuidv1(), title: 'asdf' }).then(() => {});
    axios
      .get('/api/brand')
      .then(res => {
        this.brandRd = RemoteData.success<any, Error>(res.data);
      })
      .catch(e => {
        this.brandRd = RemoteData.failure<any, Error>(e);
      });
  }
});
</script>
