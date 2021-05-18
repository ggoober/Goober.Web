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
  data: function data() {
    return {
      scopes: [],
      claims: [],
      createdDateTo: null,
      createdDateFrom: null,
      onlyActual: true,
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
      claimsList: [],
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
      errorMessage: null
    };
  },
  components: {
    'users-search-filter': _usersSearchFilter.default
  }
});

},{"./components/users-search-filter":1,"axios":"axios","vue":"vue","vue-multiselect":"vue-multiselect"}]},{},[2])
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5vZGVfbW9kdWxlcy9icm93c2VyLXBhY2svX3ByZWx1ZGUuanMiLCJDOi9wcm9qZWN0cy9nb29iZXIvR29vYmVyLldlYi9Hb29iZXIuV2ViLlZ1ZUpzLkV4YW1wbGUvd3d3cm9vdC9zcmMvanMvdXNlci9jb21wb25lbnRzL3VzZXJzLXNlYXJjaC1maWx0ZXIuanMiLCJDOi9wcm9qZWN0cy9nb29iZXIvR29vYmVyLldlYi9Hb29iZXIuV2ViLlZ1ZUpzLkV4YW1wbGUvd3d3cm9vdC9zcmMvanMvdXNlci9pbmRleC5qcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTs7Ozs7Ozs7QUNBQzs7Ozs7Ozs7ZUFFYztBQUNYLEVBQUEsUUFBUSxFQUFFLG9CQURDO0FBRVgsRUFBQSxJQUZXLGtCQUVKO0FBQ0gsV0FBTztBQUNILE1BQUEsTUFBTSxFQUFFLEVBREw7QUFFSCxNQUFBLE1BQU0sRUFBRSxFQUZMO0FBR0gsTUFBQSxhQUFhLEVBQUUsSUFIWjtBQUlILE1BQUEsZUFBZSxFQUFFLElBSmQ7QUFLSCxNQUFBLFVBQVUsRUFBRSxJQUxUO0FBT0gsTUFBQSxVQUFVLEVBQUUsQ0FDUjtBQUNJLFFBQUEsRUFBRSxFQUFFLENBRFI7QUFFSSxRQUFBLElBQUksRUFBRTtBQUZWLE9BRFEsRUFLUjtBQUNJLFFBQUEsRUFBRSxFQUFFLENBRFI7QUFFSSxRQUFBLElBQUksRUFBRTtBQUZWLE9BTFEsRUFTUjtBQUNJLFFBQUEsRUFBRSxFQUFFLENBRFI7QUFFSSxRQUFBLElBQUksRUFBRTtBQUZWLE9BVFEsRUFhUjtBQUNJLFFBQUEsRUFBRSxFQUFFLENBRFI7QUFFSSxRQUFBLElBQUksRUFBRTtBQUZWLE9BYlEsRUFpQlI7QUFDSSxRQUFBLEVBQUUsRUFBRSxDQURSO0FBRUksUUFBQSxJQUFJLEVBQUU7QUFGVixPQWpCUSxFQXFCUjtBQUNJLFFBQUEsRUFBRSxFQUFFLENBRFI7QUFFSSxRQUFBLElBQUksRUFBRTtBQUZWLE9BckJRLEVBeUJSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsQ0FEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0F6QlEsRUE2QlI7QUFDSSxRQUFBLEVBQUUsRUFBRSxDQURSO0FBRUksUUFBQSxJQUFJLEVBQUU7QUFGVixPQTdCUSxFQWlDUjtBQUNJLFFBQUEsRUFBRSxFQUFFLENBRFI7QUFFSSxRQUFBLElBQUksRUFBRTtBQUZWLE9BakNRLEVBcUNSO0FBQ0ksUUFBQSxFQUFFLEVBQUUsRUFEUjtBQUVJLFFBQUEsSUFBSSxFQUFFO0FBRlYsT0FyQ1EsQ0FQVDtBQWlESCxNQUFBLFVBQVUsRUFBRSxFQWpEVDtBQWtESCxNQUFBLGVBQWUsRUFBRTtBQWxEZCxLQUFQO0FBb0RILEdBdkRVO0FBd0RYLEVBQUEsT0FBTyxFQUFFO0FBQ0MsSUFBQSxlQURELDJCQUNpQixLQURqQixFQUN3QjtBQUFBOztBQUFBO0FBQUE7QUFBQTtBQUFBO0FBQUE7QUFBQTtBQUN6QixnQkFBQSxLQUFJLENBQUMsZUFBTCxHQUF1QixJQUF2QjtBQUR5QjtBQUFBLHVCQUdULGVBQU0sSUFBTixDQUFXLG1CQUFYLEVBQWdDO0FBQUUsa0JBQUEsU0FBUyxFQUFFLEtBQWI7QUFBb0Isa0JBQUEsUUFBUSxFQUFFLEVBQTlCO0FBQWtDLGtCQUFBLEtBQUssRUFBRTtBQUF6QyxpQkFBaEMsQ0FIUzs7QUFBQTtBQUdyQixnQkFBQSxHQUhxQjtBQUt6QixnQkFBQSxLQUFJLENBQUMsZUFBTCxHQUF1QixLQUF2Qjs7QUFFQSxvQkFBSSxHQUFHLENBQUMsTUFBSixJQUFjLEdBQWxCLEVBQXVCO0FBQ25CLGtCQUFBLEtBQUksQ0FBQyxVQUFMLEdBQWtCLEdBQUcsQ0FBQyxJQUFKLENBQVMsTUFBM0I7QUFDSCxpQkFGRCxNQUlBO0FBQ0ksa0JBQUEsS0FBSSxDQUFDLFVBQUwsR0FBa0IsRUFBbEI7QUFDSDs7QUFid0I7QUFBQTtBQUFBO0FBQUE7QUFBQTtBQUFBO0FBQUE7QUFjNUI7QUFmSTtBQXhERSxDOzs7Ozs7QUNGZDs7QUFDRDs7QUFDQTs7QUFJQTs7OztBQUZBLGFBQUksU0FBSixDQUFjLGlCQUFkLEVBQWlDLHVCQUFqQzs7QUFJQSxJQUFJLE1BQU0sR0FBRyxJQUFJLFlBQUosQ0FBUTtBQUNqQixFQUFBLEVBQUUsRUFBRSxlQURhO0FBRWpCLEVBQUEsSUFGaUIsa0JBRVY7QUFDSCxXQUFPO0FBQ0gsTUFBQSxXQUFXLEVBQUUsVUFBVSxDQUFDLFdBRHJCO0FBRUgsTUFBQSxZQUFZLEVBQUU7QUFGWCxLQUFQO0FBSUgsR0FQZ0I7QUFRakIsRUFBQSxVQUFVLEVBQUU7QUFDUiwyQkFBdUI7QUFEZjtBQVJLLENBQVIsQ0FBYiIsImZpbGUiOiJnZW5lcmF0ZWQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlc0NvbnRlbnQiOlsiKGZ1bmN0aW9uKCl7ZnVuY3Rpb24gcihlLG4sdCl7ZnVuY3Rpb24gbyhpLGYpe2lmKCFuW2ldKXtpZighZVtpXSl7dmFyIGM9XCJmdW5jdGlvblwiPT10eXBlb2YgcmVxdWlyZSYmcmVxdWlyZTtpZighZiYmYylyZXR1cm4gYyhpLCEwKTtpZih1KXJldHVybiB1KGksITApO3ZhciBhPW5ldyBFcnJvcihcIkNhbm5vdCBmaW5kIG1vZHVsZSAnXCIraStcIidcIik7dGhyb3cgYS5jb2RlPVwiTU9EVUxFX05PVF9GT1VORFwiLGF9dmFyIHA9bltpXT17ZXhwb3J0czp7fX07ZVtpXVswXS5jYWxsKHAuZXhwb3J0cyxmdW5jdGlvbihyKXt2YXIgbj1lW2ldWzFdW3JdO3JldHVybiBvKG58fHIpfSxwLHAuZXhwb3J0cyxyLGUsbix0KX1yZXR1cm4gbltpXS5leHBvcnRzfWZvcih2YXIgdT1cImZ1bmN0aW9uXCI9PXR5cGVvZiByZXF1aXJlJiZyZXF1aXJlLGk9MDtpPHQubGVuZ3RoO2krKylvKHRbaV0pO3JldHVybiBvfXJldHVybiByfSkoKSIsIu+7v2ltcG9ydCBheGlvcyBmcm9tICdheGlvcyc7XHJcblxyXG5leHBvcnQgZGVmYXVsdCB7XHJcbiAgICB0ZW1wbGF0ZTogJyN1c2Vyc1NlYXJjaEZpbHRlcicsXHJcbiAgICBkYXRhKCkge1xyXG4gICAgICAgIHJldHVybiB7XHJcbiAgICAgICAgICAgIHNjb3BlczogW10sXHJcbiAgICAgICAgICAgIGNsYWltczogW10sXHJcbiAgICAgICAgICAgIGNyZWF0ZWREYXRlVG86IG51bGwsXHJcbiAgICAgICAgICAgIGNyZWF0ZWREYXRlRnJvbTogbnVsbCxcclxuICAgICAgICAgICAgb25seUFjdHVhbDogdHJ1ZSxcclxuXHJcbiAgICAgICAgICAgIHNjb3Blc0xpc3Q6IFtcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogMSxcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiBcInNjb3BlIDFcIlxyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogMixcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiAnc2NvcGUgMidcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDMsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogJ3Njb3BlIDMnXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgIGlkOiA0LFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6ICdzY29wZSA0J1xyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogNSxcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiAnc2NvcGUgNSdcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDYsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogXCJzY29wZSA2XCJcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDcsXHJcbiAgICAgICAgICAgICAgICAgICAgbmFtZTogJ3Njb3BlIDcnXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAge1xyXG4gICAgICAgICAgICAgICAgICAgIGlkOiA4LFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6ICdzY29wZSA4J1xyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgICAgICBpZDogOSxcclxuICAgICAgICAgICAgICAgICAgICBuYW1lOiAnc2NvcGUgOSdcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWQ6IDEwLFxyXG4gICAgICAgICAgICAgICAgICAgIG5hbWU6ICdzY29wZSAxMCdcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgXSxcclxuICAgICAgICAgICAgY2xhaW1zTGlzdDogW10sXHJcbiAgICAgICAgICAgIGlzQ2xhaW1zTG9hZGluZzogZmFsc2VcclxuICAgICAgICB9O1xyXG4gICAgfSxcclxuICAgIG1ldGhvZHM6IHtcclxuICAgICAgICBhc3luYyBmaW5kQ2xhaW1zQXN5bmMocXVlcnkpIHtcclxuICAgICAgICAgICAgdGhpcy5pc0NsYWltc0xvYWRpbmcgPSB0cnVlO1xyXG5cclxuICAgICAgICAgICAgdmFyIHJlcyA9IGF3YWl0IGF4aW9zLnBvc3QoJy9hcGkvY2xhaW0vc2VhcmNoJywgeyB0ZXh0UXVlcnk6IHF1ZXJ5LCBzY29wZUlkczogW10sIGNvdW50OiAxNSB9KTtcclxuXHJcbiAgICAgICAgICAgIHRoaXMuaXNDbGFpbXNMb2FkaW5nID0gZmFsc2U7XHJcblxyXG4gICAgICAgICAgICBpZiAocmVzLnN0YXR1cyA9PSAyMDApIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuY2xhaW1zTGlzdCA9IHJlcy5kYXRhLmNsYWltcztcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICBlbHNlXHJcbiAgICAgICAgICAgIHtcclxuICAgICAgICAgICAgICAgIHRoaXMuY2xhaW1zTGlzdCA9IFtdO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG59OyIsIu+7v2ltcG9ydCBWdWUgZnJvbSAndnVlJztcclxuaW1wb3J0IGF4aW9zIGZyb20gJ2F4aW9zJztcclxuaW1wb3J0IE11bHRpc2VsZWN0IGZyb20gJ3Z1ZS1tdWx0aXNlbGVjdCdcclxuXHJcblZ1ZS5jb21wb25lbnQoJ3Z1ZS1tdWx0aXNlbGVjdCcsIE11bHRpc2VsZWN0KVxyXG5cclxuaW1wb3J0IHVzZXJzU2VhcmNoRmlsdGVyIGZyb20gJy4vY29tcG9uZW50cy91c2Vycy1zZWFyY2gtZmlsdGVyJztcclxuXHJcbnZhciB2dWVBcHAgPSBuZXcgVnVlKHtcclxuICAgIGVsOiAnI3VzZXJzLXNlYXJjaCcsXHJcbiAgICBkYXRhKCkge1xyXG4gICAgICAgIHJldHVybiB7XHJcbiAgICAgICAgICAgIGJhY2tlbmRVcmxzOiBnbG9iYWxUaGlzLmJhY2tlbmRVcmxzLFxyXG4gICAgICAgICAgICBlcnJvck1lc3NhZ2U6IG51bGwsXHJcbiAgICAgICAgfTtcclxuICAgIH0sXHJcbiAgICBjb21wb25lbnRzOiB7XHJcbiAgICAgICAgJ3VzZXJzLXNlYXJjaC1maWx0ZXInOiB1c2Vyc1NlYXJjaEZpbHRlclxyXG4gICAgfVxyXG59KTsiXX0=
