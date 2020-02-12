<template functional>
  <div id="app">
    <div id="nav">
      <router-link to="/">Sachbearbeiter</router-link>&nbsp;|
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
  border: 1px solid;
  padding: 10px;
}
legend {
  width: 105px;
}
.entry {
  margin-bottom: 5px;
}
.row {
  margin: 2px;
}
.form-group {
  padding-top: 10px;
}
.even {
  background: white;
}
.odd {
  background: lightgray;
}
</style>
