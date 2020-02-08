<template functional>
  <div class="spinner">
    <em class="fas fa-spinner fa-spin text-primary" />
  </div>
</template>

<script>
export default {
  name: 'loading'
};
</script>
<style scoped>
.fas.fa-spinner {
  font-size: 36px;
}

.spinner {
  margin: 24px auto;
  width: 70px;
  text-align: center;
}
</style>
