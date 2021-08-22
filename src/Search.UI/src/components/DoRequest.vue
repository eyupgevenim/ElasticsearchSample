<template>
    <div class="do-request">
        <b-row>
            <b-col lg="12" md="12" sm="12">
                <b-row>
                    <b-col md="6" class="pl-2">
                        <div class="ml-2">
                            <template>
                                <b-container fluid class="mt-2">

                                    <b-row class="methods m-1">
                                        <b-col sm="4">
                                            <label for="methods">Method:</label>
                                        </b-col>
                                        <b-col sm="8">
                                            <b-form-select id="methods" v-model="method" :options="methods"></b-form-select>
                                        </b-col>
                                    </b-row>

                                    <b-row class="path m-1">
                                        <b-col sm="4">
                                            <label for="path">Path:</label>
                                        </b-col>
                                        <b-col sm="8">
                                            <b-form-input id="path" type="search" v-model="path"></b-form-input>
                                        </b-col>
                                    </b-row>

                                    <b-row class="data m-1">
                                        <b-col sm="4">
                                            <label for="path">Data:</label>
                                        </b-col>
                                        <b-col sm="8">
                                            <b-form-textarea id="data" v-model="data" rows="10" style="min-height: 350px;"></b-form-textarea>
                                        </b-col>
                                    </b-row>

                                    <hr />

                                    <b-row class="m-2">
                                        <b-col>
                                            <template>
                                                <div>
                                                    <b-button @click="execute()" variant="light">Execute</b-button>
                                                </div>
                                            </template>
                                        </b-col>
                                    </b-row>

                                </b-container>
                            </template>
                        </div>
                    </b-col>
                    <b-col md="6">
                        <b-col class="ml-2">
                            <template>
                                <div>
                                    <!--<code> {{responseData}} </code>-->
                                    <pre style="background-color:#fbfbfb;"> {{responseData}} </pre>
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
    const defaultJsonQuery = {
        "sort": [
            {
                "price": {
                    "order": "desc"
                }
            }
        ],
        "query": {
            "match_all": {}
        }
    };

    export default {
        name: 'DoRequest',
        data() {
            return {
                method: '0',
                path: 'product/_search',
                data: JSON.stringify(defaultJsonQuery, undefined, 2),
                methods: [
                    { text: 'GET', value: '0' },
                    { text: 'POST', value: '1' },
                    { text: 'PUT', value: '2' },
                    { text: 'DELETE', value: '3' },
                    { text: 'HEAD', value: '4' },
                ],
                responseData: {}
            };
        },
        methods: {
            execute() {
                SearchService.doRequest({
                    method: this.method,
                    path: this.path,
                    data: this.data//JSON.stringify(this.data)
                }).then(response => {
                    console.log("response:", response);
                    if (response.status == 200) {
                        this.responseData = response.data;
                    }
                }, error => {
                    console.log("error:", error);
                });
            }
        }
    };
</script>

<style scoped>
</style>