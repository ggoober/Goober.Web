import Vue from 'vue';
import axios from 'axios';
import Multiselect from 'vue-multiselect'

Vue.component('vue-multiselect', Multiselect)

import usersSearchFilter from './components/users-search-filter';

var vueApp = new Vue({
    el: '#users-search',
    data() {
        return {
            backendUrls: globalThis.backendUrls,
            errorMessage: null,
            searchFilter: {
                createdDateTo: null,
                createdDateFrom: null,
                errorMessage: null,
                scopes: [{ id: 1, name: "scope 1"}],
                claims: [],
                onlyActual: true
            }
        };
    },
    components: {
        'users-search-filter': usersSearchFilter
    },
    methods: {
        searchFilterChange(event) {
            console.log('searchFilterChange, event:' + JSON.stringify(event));
            this.searchFilter.scopes = event;
        }
    }
});