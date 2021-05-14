import axios from 'axios';

export default {
    template: '#usersSearchFilter',
    props: {
        value: {
            type: Object,
            default: function () {
                return {
                    createdDateFrom: null,
                    errorMessage: null,
                    createdDateTo: null,
                    scopes: null
                };
            },
        }
    },
    data() {
        return {
            createdDateTo: null,
            createdDateFrom: null,
            errorMessage: null,
            scopes: null,
            scopesList: [
                {
                    id: 1,
                    name: "name 1"
                },
                {
                    id: 2,
                    name: 'name 2'
                },
                {
                    id: 3,
                    name: 'name 3'
                },
                {
                    id: 4,
                    name: 'name 4'
                },
                {
                    id: 5,
                    name: 'name 5'
                }
            ]
        };
    }
};