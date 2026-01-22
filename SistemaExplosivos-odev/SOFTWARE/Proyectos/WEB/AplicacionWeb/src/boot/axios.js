import { defineBoot } from '#q-app/wrappers';
import axios from 'axios';
import { axiosApiInstancia } from 'src/services/AxiosService.js';

export default defineBoot(({ app }) => {
  // Configura axios globalmente
  app.config.globalProperties.$axios = axios;
  app.config.globalProperties.$api = axiosApiInstancia;
});

export { axios, axiosApiInstancia };

// TODO: Cambiar el cancellation token por una solucion mas eficiente de axios https://axios-http.com/es/docs/cancellation
