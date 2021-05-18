import Vue from 'vue';
import axios from 'axios';
import Multiselect from 'vue-multiselect'

Vue.component('vue-multiselect', Multiselect)

import vc_usersSearchFilter from './components/users-search-filter';

var vueApp = new Vue({
    el: '#users-search',
    data() {
        return {
            backendUrls: globalThis.backendUrls,
            errorMessage: null,
            searchFilter: null
        };
    },
    mounted: function ()
    {
        this.searchFilter = this.$refs.usersSearchFilter.getFilter();

        this.searchFilter.scopes = [{ id: 3, name: "scope 3" }];
    },
    methods:
    {
        findClick: function () {
            console.log('findClick: ' + JSON.stringify(this.searchFilter));

            searchFilter.scopes = [{ id: 3, name: "scope 3" }];
        }
    },
    components: {
        'users-search-filter': vc_usersSearchFilter
    }
});