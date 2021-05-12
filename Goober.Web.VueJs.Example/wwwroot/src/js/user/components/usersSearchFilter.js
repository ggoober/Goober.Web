import axios from 'axios';

export default {
    template: '#usersSearchFilter',
    props: {
        value: {
            type: Object,
            default: function () {
                return {
                    createdDateFrom: null
                };
            },
        }
    },
    data() {
        return {
            createdDateFrom: null,
            errorMessage: null,
        };
    },
    methods: {
        input() {
            this.$emit('input', {
                createdDateFrom: this.createdDateFrom,
            });
        }
    }
};