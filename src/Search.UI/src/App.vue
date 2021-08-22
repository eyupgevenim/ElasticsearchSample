<template>
    <div id="app">

        <b-jumbotron>
            <b-row class="text-center mb-2">
                <b-col lg="12" md="12" sm="12">
                    <h4>Search UI with Elasticsearch</h4>
                </b-col>
            </b-row>
            <b-row>

                <b-col lg="12" md="12" sm="12">

                    <b-card no-body>
                        <b-tabs card>
                            <b-tab no-body title="Search Product Name">
                                <SearchProductName />
                            </b-tab>

                            <b-tab no-body title="Advanced Search Product">
                                <AdvancedSearchProduct />
                            </b-tab>

                            <b-tab no-body title="Do Request">
                                <DoRequest />
                            </b-tab>

                        </b-tabs>
                    </b-card>

                </b-col>
                
            </b-row>

        </b-jumbotron>

    </div>
</template>

<script>
    import SearchProductName from './components/SearchProductName.vue';
    import AdvancedSearchProduct from './components/AdvancedSearchProduct.vue';
    import DoRequest from './components/DoRequest.vue';

    export default {
        name: 'app',
        components: {
            SearchProductName,
            AdvancedSearchProduct,
            DoRequest
        }
    };
</script>

<style>
</style>

