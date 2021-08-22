import axios from 'axios';

const API_URL = 'http://localhost:5000/api/v1';

const SearchService = {
    searchAll() {
        return axios.get(`${API_URL}/Search`);
    },
    search(filterOption) {
        return axios.post(`${API_URL}/Search`, filterOption);
    },
    queryJson(query) {
        return axios.post(`${API_URL}/Search/QueryJson`, query);
    },
    doRequest(request) {
        return axios.post(`${API_URL}/Search/DoRequest`, request);
    }
};

export default SearchService;