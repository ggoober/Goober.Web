(function(){function r(e,n,t){function o(i,f){if(!n[i]){if(!e[i]){var c="function"==typeof require&&require;if(!f&&c)return c(i,!0);if(u)return u(i,!0);var a=new Error("Cannot find module '"+i+"'");throw a.code="MODULE_NOT_FOUND",a}var p=n[i]={exports:{}};e[i][0].call(p.exports,function(r){var n=e[i][1][r];return o(n||r)},p,p.exports,r,e,n,t)}return n[i].exports}for(var u="function"==typeof require&&require,i=0;i<t.length;i++)o(t[i]);return o}return r})()({1:[function(require,module,exports){
"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.default = void 0;

var _axios = _interopRequireDefault(require("axios"));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function asyncGeneratorStep(gen, resolve, reject, _next, _throw, key, arg) { try { var info = gen[key](arg); var value = info.value; } catch (error) { reject(error); return; } if (info.done) { resolve(value); } else { Promise.resolve(value).then(_next, _throw); } }

function _asyncToGenerator(fn) { return function () { var self = this, args = arguments; return new Promise(function (resolve, reject) { var gen = fn.apply(self, args); function _next(value) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "next", value); } function _throw(err) { asyncGeneratorStep(gen, resolve, reject, _next, _throw, "throw", err); } _next(undefined); }); }; }

var _default = {
  template: '#usersSearchFilter',
  props: {
    createdDateTo: Date,
    createdDateFrom: Date,
    errorMessage: String,
    scopes: Array,
    claims: Array,
    onlyActual: Boolean
  },
  data: function data() {
    return {
      scopesList: [{
        id: 1,
        name: "scope 1"
      }, {
        id: 2,
        name: 'scope 2'
      }, {
        id: 3,
        name: 'scope 3'
      }, {
        id: 4,
        name: 'scope 4'
      }, {
        id: 5,
        name: 'scope 5'
      }, {
        id: 6,
        name: "scope 6"
      }, {
        id: 7,
        name: 'scope 7'
      }, {
        id: 8,
        name: 'scope 8'
      }, {
        id: 9,
        name: 'scope 9'
      }, {
        id: 10,
        name: 'scope 10'
      }],
      selectedScopes: [],
      claimsList: [],
      selectedClaims: [],
      isClaimsLoading: false
    };
  },
  methods: {
    findClaimsAsync: function findClaimsAsync(query) {
      var _this = this;

      return _asyncToGenerator( /*#__PURE__*/regeneratorRuntime.mark(function _callee() {
        var res;
        return regeneratorRuntime.wrap(function _callee$(_context) {
          while (1) {
            switch (_context.prev = _context.next) {
              case 0:
                _this.isClaimsLoading = true;
                _context.next = 3;
                return _axios.default.post('/api/claim/search', {
                  textQuery: query,
                  scopeIds: [],
                  count: 15
                });

              case 3:
                res = _context.sent;
                _this.isClaimsLoading = false;

                if (res.status == 200) {
                  _this.claimsList = res.data.claims;
                } else {
                  _this.claimsList = [];
                }

              case 6:
              case "end":
                return _context.stop();
            }
          }
        }, _callee);
      }))();
    }
  },
  watch: {
    selectedScopes: function selectedScopes(oldValue, newValue) {
      this.$emit('change:scopes', newValue);
    }
  },
  computed: {
    scopesComputed: {
      get: function get() {
        return this.scopes;
      },
      set: function set(newValue) {
        console.log('scopesComputed:set' + JSON.stringify(newValue));
        this.$emit('change:scopes', newValue);
      }
    }
  }
};
exports.default = _default;

},{"axios":"axios"}],2:[function(require,module,exports){
"use strict";

var _vue = _interopRequireDefault(require("vue"));

var _axios = _interopRequireDefault(require("axios"));

var _vueMultiselect = _interopRequireDefault(require("vue-multiselect"));

var _usersSearchFilter = _interopRequireDefault(require("./components/users-search-filter"));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

_vue.default.component('vue-multiselect', _vueMultiselect.default);

var vueApp = new _vue.default({
  el: '#users-search',
  data: function data() {
    return {
      backendUrls: globalThis.backendUrls,
      errorMessage: null,
      searchFilter: {
        createdDateTo: null,
        createdDateFrom: null,
        errorMessage: null,
        scopes: [{
          id: 1,
          name: "scope 1"
        }],
        claims: [],
        onlyActual: true
      }
    };
  },
  components: {
    'users-search-filter': _usersSearchFilter.default
  },
  methods: {
    searchFilterChange: function searchFilterChange(event) {
      console.log('searchFilterChange, event:' + JSON.stringify(event));
      this.searchFilter.scopes = event;
    }
  }
});

},{"./components/users-search-filter":1,"axios":"axios","vue":"vue","vue-multiselect":"vue-multiselect"}]},{},[2])
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJDOi9wcm9qZWN0cy9nb29iZXIvR29vYmVyLldlYi9Hb29iZXIuV2ViLlZ1ZUpzLkV4YW1wbGUvd3d3cm9vdC9zcmMvanMvdXNlci9jb21wb25lbnRzL3VzZXJzLXNlYXJjaC1maWx0ZXIuanMiLCJDOi9wcm9qZWN0cy9nb29iZXIvR29vYmVyLldlYi9Hb29iZXIuV2ViLlZ1ZUpzLkV4YW1wbGUvd3d3cm9vdC9zcmMvanMvdXNlci9pbmRleC5qcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTs7Ozs7Ozs7QUNBQzs7Ozs7Ozs7ZUFFYztBQUNYLEVBQUEsUUFBUSxFQUFFLG9CQURDO0FBRVgsRUFBQSxLQUFLLEVBQUU7QUFDSCxJQUFBLGFBQWEsRUFBRSxJQURaO0FBRUgsSUFBQSxlQUFlLEVBQUUsSUFGZDtBQUdILElBQUEsWUFBWSxFQUFFLE1BSFg7QUFJSCxJQUFBLE1BQU0sRUFBRSxLQUpMO0FBS0gsSUFBQSxNQUFNLEVBQUUsS0FMTDtBQU1ILElBQUEsVUFBVSxFQUFFO0FBTlQsR0FGSTtBQVVYLEVBQUEsSUFWVyxrQkFVSjtBQUNILFdBQU87QUFDSCxNQUFBLFVBQVUsRUFBRSxDQUNSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FEUSxFQUtSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FMUSxFQVNSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FUUSxFQWFSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FiUSxFQWlCUjtBQUNJLFFBQUEsRUFBRSxFQUFFLENBRFI7QUFFSSxRQUFBLElBQUksRUFBRTtBQUZWLE9BakJRLEVBcUJSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtB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QVA7QUFZSCxHQWZnQjtBQWdCakIsRUFBQSxVQUFVLEVBQUU7QUFDUiwyQkFBdUI7QUFEZixHQWhCSztBQW1CakIsRUFBQSxPQUFPLEVBQUU7QUFDTCxJQUFBLGtCQURLLDhCQUNjLEtBRGQsRUFDcUI7QUFDdEIsTUFBQSxPQUFPLENBQUMsR0FBUixDQUFZLCtCQUErQixJQUFJLENBQUMsU0FBTCxDQUFlLEtBQWYsQ0FBM0M7QUFDQSxXQUFLLFlBQUwsQ0FBa0IsTUFBbEIsR0FBMkIsS0FBM0I7QUFDSDtBQUpJO0FBbkJRLENBQVIsQ0FBYiIsImZpbGUiOiJnZW5lcmF0ZWQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlc0NvbnRlbnQiOlsiKGZ1bmN0aW9uKCl7ZnVuY3Rpb24gcihlLG4sdCl7ZnVuY3Rpb24gbyhpLGYpe2lmKCFuW2ldKXtpZighZVtpXSl7dmFyIGM9XCJmdW5jdGlvblwiPT10eXBlb2YgcmVxdWlyZSYmcmVxdWlyZTtpZighZiYmYylyZXR1cm4gYyhpLCEwKTtpZih1KXJldHVybiB1KGksITApO3ZhciBhPW5ldyBFcnJvcihcIkNhbm5vdCBmaW5kIG1vZHVsZSAnXCIraStcIidcIik7dGhyb3cgYS5jb2RlPVwiTU9EVUxFX05PVF9GT1VORFwiLGF9dmFyIHA9bltpXT17ZXhwb3J0czp7fX07ZVtpXVswXS5jYWxsKHAuZXhwb3J0cyxmdW5jdGlvbihyKXt2YXIgbj1lW2ldWzFdW3JdO3JldHVybiBvKG58fHIpfSxwLHAuZXhwb3J0cyxyLGUsbix0KX1yZXR1cm4gbltpXS5leHBvcnRzfWZvcih2YXIgdT1cImZ1bmN0aW9uXCI9PXR5cGVvZiByZXF1aXJlJiZyZXF1aXJlLGk9MDtpPHQubGVuZ3RoO2krKylvKHRbaV0pO3JldHVybiBvfXJldHVybiByfSkoKSIsIu+7v2ltcG9ydCBheGlvcyBmcm9tICdheGlvcyc7XHJcblxyXG5leHBvcnQgZGVmYXVsdCB7XHJcbiAgICB0ZW1wbGF0ZTogJyN1c2Vyc1NlYXJjaEZpbHRlcicsXHJcbiAgICBwcm9wczoge1xyXG4gICAgICAgIGNyZWF0ZWREYXRlVG86IERhdGUsXHJcbiAgICAgICAgY3JlYXRlZERhdGVGcm9tOiBEYXRlLFxyXG4gICAgICAgIGVycm9yTWVzc2FnZTogU3RyaW5nLFxyXG4gICAgICAgIHNjb3BlczogQXJyYXksXHJcbiAgICAgICAgY2xhaW1zOiBBcnJheSxcclxuICAgICAgICBvbmx5QWN0dWFsOiBCb29sZWFuXHJcbiAgICB9LFxyXG4gICAgZGF0YSgpIHtcclxuICAgICAgICByZXR1cm4ge1xyXG4gICAgICAgICAgICBzY29wZXNMaXN0OiBbXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDEsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogXCJzY29wZSAxXCJcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDIsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogJ3Njb3BlIDInXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgIGlkOiAzLFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6ICdzY29wZSAzJ1xyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogNCxcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiAnc2NvcGUgNCdcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDUsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogJ3Njb3BlIDUnXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgIGlkOiA2LFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6IFwic2NvcGUgNlwiXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgIGlkOiA3LFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6ICdzY29wZSA3J1xyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogOCxcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiAnc2NvcGUgOCdcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDksXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogJ3Njb3BlIDknXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgIGlkOiAxMCxcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiAnc2NvcGUgMTAnXHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIF0sXHJcbiAgICAgICAgICAgIHNlbGVjdGVkU2NvcGVzOiBbXSxcclxuXHJcbiAgICAgICAgICAgIGNsYWltc0xpc3Q6IFtdLFxyXG4gICAgICAgICAgICBzZWxlY3RlZENsYWltczogW10sXHJcbiAgICAgICAgICAgIGlzQ2xhaW1zTG9hZGluZzogZmFsc2VcclxuICAgICAgICB9O1xyXG4gICAgfSxcclxuICAgIG1ldGhvZHM6IHtcclxuICAgICAgICBhc3luYyBmaW5kQ2xhaW1zQXN5bmMocXVlcnkpIHtcclxuICAgICAgICAgICAgdGhpcy5pc0NsYWltc0xvYWRpbmcgPSB0cnVlO1xyXG5cclxuICAgICAgICAgICAgdmFyIHJlcyA9IGF3YWl0IGF4aW9zLnBvc3QoJy9hcGkvY2xhaW0vc2VhcmNoJywgeyB0ZXh0UXVlcnk6IHF1ZXJ5LCBzY29wZUlkczogW10sIGNvdW50OiAxNSB9KTtcclxuXHJcbiAgICAgICAgICAgIHRoaXMuaXNDbGFpbXNMb2FkaW5nID0gZmFsc2U7XHJcblxyXG4gICAgICAgICAgICBpZiAocmVzLnN0YXR1cyA9PSAyMDApIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuY2xhaW1zTGlzdCA9IHJlcy5kYXRhLmNsYWltcztcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICBlbHNlXHJcbiAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuY2xhaW1zTGlzdCA9IFtdO1xyXG4gICAgICAgICAgICB9XHJcblxyXG4gICAgICAgIH1cclxuICAgIH0sXHJcbiAgICB3YXRjaDoge1xyXG4gICAgICAgIHNlbGVjdGVkU2NvcGVzKG9sZFZhbHVlLCBuZXdWYWx1ZSkge1xyXG4gICAgICAgICAgICBcclxuICAgICAgICAgICAgdGhpcy4kZW1pdCgnY2hhbmdlOnNjb3BlcycsIG5ld1ZhbHVlKVxyXG4gICAgICAgIH1cclxuICAgIH0sXHJcbiAgICBjb21wdXRlZDpcclxuICAgIHtcclxuICAgICAgICBzY29wZXNDb21wdXRlZDoge1xyXG4gICAgICAgICAgICBnZXQ6IGZ1bmN0aW9uICgpIHtcclxuICAgICAgICAgICAgICAgIHJldHVybiB0aGlzLnNjb3BlcztcclxuICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgc2V0OiBmdW5jdGlvbiAobmV3VmFsdWUpIHtcclxuICAgICAgICAgICAgICAgIGNvbnNvbGUubG9nKCdzY29wZXNDb21wdXRlZDpzZXQnICsgSlNPTi5zdHJpbmdpZnkobmV3VmFsdWUpKTtcclxuICAgICAgICAgICAgICAgIHRoaXMuJGVtaXQoJ2NoYW5nZTpzY29wZXMnLCBuZXdWYWx1ZSk7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICB9XHJcbn07Iiwi77u/aW1wb3J0IFZ1ZSBmcm9tICd2dWUnO1xyXG5pbXBvcnQgYXhpb3MgZnJvbSAnYXhpb3MnO1xyXG5pbXBvcnQgTXVsdGlzZWxlY3QgZnJvbSAndnVlLW11bHRpc2VsZWN0J1xyXG5cclxuVnVlLmNvbXBvbmVudCgndnVlLW11bHRpc2VsZWN0JywgTXVsdGlzZWxlY3QpXHJcblxyXG5pbXBvcnQgdXNlcnNTZWFyY2hGaWx0ZXIgZnJvbSAnLi9jb21wb25lbnRzL3VzZXJzLXNlYXJjaC1maWx0ZXInO1xyXG5cclxudmFyIHZ1ZUFwcCA9IG5ldyBWdWUoe1xyXG4gICAgZWw6ICcjdXNlcnMtc2VhcmNoJyxcclxuICAgIGRhdGEoKSB7XHJcbiAgICAgICAgcmV0dXJuIHtcclxuICAgICAgICAgICAgYmFja2VuZFVybHM6IGdsb2JhbFRoaXMuYmFja2VuZFVybHMsXHJcbiAgICAgICAgICAgIGVycm9yTWVzc2FnZTogbnVsbCxcclxuICAgICAgICAgICAgc2VhcmNoRmlsdGVyOiB7XHJcbiAgICAgICAgICAgICAgICBjcmVhdGVkRGF0ZVRvOiBudWxsLFxyXG4gICAgICAgICAgICAgICAgY3JlYXRlZERhdGVGcm9tOiBudWxsLFxyXG4gICAgICAgICAgICAgICAgZXJyb3JNZXNzYWdlOiBudWxsLFxyXG4gICAgICAgICAgICAgICAgc2NvcGVzOiBbeyBpZDogMSwgbmFtZTogXCJzY29wZSAxXCJ9XSxcclxuICAgICAgICAgICAgICAgIGNsYWltczogW10sXHJcbiAgICAgICAgICAgICAgICBvbmx5QWN0dWFsOiB0cnVlXHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9O1xyXG4gICAgfSxcclxuICAgIGNvbXBvbmVudHM6IHtcclxuICAgICAgICAndXNlcnMtc2VhcmNoLWZpbHRlcic6IHVzZXJzU2VhcmNoRmlsdGVyXHJcbiAgICB9LFxyXG4gICAgbWV0aG9kczoge1xyXG4gICAgICAgIHNlYXJjaEZpbHRlckNoYW5nZShldmVudCkge1xyXG4gICAgICAgICAgICBjb25zb2xlLmxvZygnc2VhcmNoRmlsdGVyQ2hhbmdlLCBldmVudDonICsgSlNPTi5zdHJpbmdpZnkoZXZlbnQpKTtcclxuICAgICAgICAgICAgdGhpcy5zZWFyY2hGaWx0ZXIuc2NvcGVzID0gZXZlbnQ7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG59KTsiXX0=
