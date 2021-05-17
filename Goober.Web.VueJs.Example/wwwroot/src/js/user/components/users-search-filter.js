import axios from 'axios';

export default {
    template: '#usersSearchFilter',
    props: {
        createdDateTo: Date,
        createdDateFrom: Date,
        errorMessage: String,
        scopes: Array,
        claims: Array,
        onlyActual: Boolean
    },
    data() {
        return {
            scopesList: [
                {
                    id: 1,
                    name: "scope 1"
                },
                {
                    id: 2,
                    name: 'scope 2'
                },
                {
                    id: 3,
                    name: 'scope 3'
                },
                {
                    id: 4,
                    name: 'scope 4'
                },
                {
                    id: 5,
                    name: 'scope 5'
                },
                {
                    id: 6,
                    name: "scope 6"
                },
                {
                    id: 7,
                    name: 'scope 7'
                },
                {
                    id: 8,
                    name: 'scope 8'
                },
                {
                    id: 9,
                    name: 'scope 9'
                },
                {
                    id: 10,
                    name: 'scope 10'
                }
            ],
            claimsList: [],
            isClaimsLoading: false
        };
    },
    methods: {
        async findClaimsAsync(query) {
            this.isClaimsLoading = true;

            var res = await axios.post('/api/claim/search', { textQuery: query, scopeIds: [], count: 15 });

            this.isClaimsLoading = false;

            if (res.status == 200) {
                this.claimsList = res.data.claims;
            }
            else
            {
                this.claimsList = [];
            }

        }
    },
    watch: {
        selectedScopes(oldValue, newValue) {
            
            this.$emit('change:scopes', newValue)
        }
    },
    computed:
    {
        scopesComputed: {
            get: function () {
                return this.scopes;
            },
            set: function (newValue) {
                console.log('scopesComputed:set' + JSON.stringify(newValue));
                this.$emit('change:scopes', newValue);
            }
        }
    }
};