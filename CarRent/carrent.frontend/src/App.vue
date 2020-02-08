<template>
  <div id="app">
    <div id="nav">
      <router-link to="/">Home</router-link>&nbsp;|
      <router-link to="/brand">Marken</router-link>&nbsp;|
      <router-link to="/category">Kategorien</router-link>&nbsp;|
      <router-link to="/model">Modelle</router-link>&nbsp;|
      <router-link to="/car">Autos</router-link>
    </div>
    <router-view />
  </div>
</template>

<style lang="scss">
#app {
  font-family: Arial, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}

#nav {
  padding: 30px;
}
a {
  font-weight: bold;
  color: #2c3e50;
}
a:active {
  color: #42b983;
}
fieldset {
  border: 2px solid;
  padding: 10px;
}
legend {
  width: 105px;
}
button {
  margin-top: 10px;
}
.entry {
  margin-bottom: 10px;
}
</style>
