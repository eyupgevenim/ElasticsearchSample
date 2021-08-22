<template>
    <div class="search-product-name">
        <b-row>
            <b-col lg="12" md="12" sm="12" style="min-height:400px;">

                <b-row>
                    <b-col md="3">
                        <b-card class="text-center">
                            <b-row >
                                <b-col sm="9">
                                    <b-form-input v-model="text" placeholder="search product name"></b-form-input>
                                </b-col>
                                <b-col sm="3">
                                    <b-button @click="search()" variant="light">Search</b-button>
                                </b-col>
                            </b-row>
                        </b-card>
                    </b-col>
                </b-row>

                <b-row>
                    <b-col>
                        <template>
                            <div>
                                <b-table striped hover :items="items"></b-table>
                            </div>
                        </template>
                    </b-col>
                </b-row>

            </b-col>
        </b-row>
    </div>
</template>

<script>
    import SearchService from '../services/search.service';
    export default {
        name: 'SearchProductName',
        data() {
            return {
                text: '',
                items:[]
            };
        },
        methods: {
            search() {
                SearchService.search({
                    "filters": { "product.name": [this.text]},
                    "from": 0,
                    "size": 100
                }).then(response => {
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