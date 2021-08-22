<template>
    <div class="advanced-search-product">
        <b-row>
            <b-col lg="12" md="12" sm="12">
                <b-row>
                    <b-col md="3" class="pl-2">
                        <div class="ml-2">
                            <template>
                                <b-container fluid class="mt-2">

                                    <b-row class="my-name m-1">
                                        <b-col sm="3">
                                            <label for="type-search">Name:</label>
                                        </b-col>
                                        <b-col sm="9">
                                            <b-form-input id="type-search" type="search" v-model="search"></b-form-input>
                                        </b-col>
                                    </b-row>

                                    <b-row class="my-number m-1">
                                        <b-col sm="6">
                                            <label for="type-number">Min:</label>
                                            <b-form-input id="type-number-min" type="number" v-model="priceMin" min="0"></b-form-input>
                                        </b-col>
                                        <b-col sm="6">
                                            <label for="type-number">Max:</label>
                                            <b-form-input id="type-number-max" type="number" v-model="priceMax" min="0"></b-form-input>
                                        </b-col>
                                    </b-row>

                                    <hr />

                                    <b-row class="my-sort-price m-1">
                                        <b-col sm="3">
                                            <label for="type-sort-price">Price Sort:</label>
                                        </b-col>
                                        <b-col sm="9">
                                            <b-form-select id="bg-sort-price" v-model="priceSort" :options="sortOptions"></b-form-select>
                                        </b-col>
                                    </b-row>

                                    <b-row class="my-sort-name m-1">
                                        <b-col sm="3">
                                            <label for="type-sort-name">Name Sort:</label>
                                        </b-col>
                                        <b-col sm="9">
                                            <b-form-select id="bg-sort-name" v-model="nameSort" :options="sortOptions"></b-form-select>
                                        </b-col>
                                    </b-row>

                                    <b-row class="m-2">
                                        <b-col>
                                            <template>
                                                <div>
                                                    <b-button @click="Search()" variant="light">Search</b-button>
                                                </div>
                                            </template>
                                        </b-col>
                                    </b-row>

                                </b-container>
                            </template>
                        </div>
                    </b-col>
                    <b-col md="9">
                        <b-col>
                            <template>
                                <div>
                                    <b-table striped hover :items="items"></b-table>
                                </div>
                            </template>
                        </b-col>
                    </b-col>
                </b-row>
            </b-col>
        </b-row>
    </div>
</template>

<script>
    import SearchService from '../services/search.service';
    export default {
        name: 'AdvancedSearchProduct',
        data() {
            return {
                search: '',
                items: [],

                priceMin: 85.00,
                priceMax: 100.00,

                priceSort: '',
                nameSort: '',
                sortOptions: [
                    { text: 'None', value: '' },
                    { text: 'Ascending', value: 0 },
                    { text: 'Descending', value: 1 }
                ]
            };
        },
        methods: {
            Search() {

                let searchOptions = {
                    "filters": {
                        "product.name": [ this.search ],
                        "product.price": [this.priceMin + '', this.priceMax + '']
                    },
                    "sorts": { },
                    "from": 0,
                    "size": 10
                };

                if (this.priceSort !== '') {
                    searchOptions.sorts["product.price"] = this.priceSort;
                }

                if (this.nameSort !== '') {
                    searchOptions.sorts["product.name"] = this.nameSort;
                }

                console.log("searchOptions:", searchOptions);

                SearchService.search(searchOptions).then(response => {
                    console.log("response:", response);
                    if (response.status == 200) {
                        this.items = response.data;
                    }
                }, error => {
                    console.log("error:", error);
                });
            }
        }
    };
</script>