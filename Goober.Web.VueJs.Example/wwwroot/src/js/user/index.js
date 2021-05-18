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
        };
    },
    mounted: function ()
    {
        this.$refs.searchFilter.filter.scopes = [{ id: 3, name: "scope 3" }];
    },
    methods:
    {
        findClick: function () {
            var searchFilter = this.$refs.searchFilter.getFilter();

            console.log('findClick: ' + JSON.stringify(searchFilter));
        }
    },
    components: {
        'users-search-filter': usersSearchFilter
    }
});