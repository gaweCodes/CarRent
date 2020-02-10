<template>
  <div class="container">
    <h1>Funktionen</h1>
    <ul>
      <li>
        <router-link to="/brand">Marken</router-link>
      </li>
      <li>
        <router-link to="/category">Kategorien</router-link>
      </li>
      <li>
        <router-link to="/model">Modelle</router-link>
      </li>
      <li>
        <router-link to="/car">Autos</router-link>
      </li>
      <li>
        <router-link to="/customers">Kunden</router-link>
      </li>
      <li>
        <router-link to="/reservations">Reservationen</router-link>
      </li>
    </ul>
  </div>
</template>

<script>
// @ is an alias to /src
import HelloWorld from '@/components/HelloWorld.vue';

export default {
  name: 'home',
  components: {
    HelloWorld
  }
};
</script>
