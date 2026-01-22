import { defineBoot } from '#q-app/wrappers';
import { i18n, traducir } from 'src/services/TranslationService.js'

export default defineBoot(({ app }) => {
  // Set i18n instance on app
  app.use(i18n);
  app.config.globalProperties.$traducir = traducir;
  app.provide('traducir', traducir);
});
