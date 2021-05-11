import '@babel/polyfill'
import Vue from 'vue'
import './plugins/bootstrap-vue';
import './plugins/vue-async-computed';

Vue.config.productionTip = false;

// Фикс globalThis для старых браузеров
let globalObject = ((0, eval)('this'));
globalObject.globalThis = globalObject;