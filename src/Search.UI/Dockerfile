# build stage
FROM node:12 as build-stage
WORKDIR /app
#COPY ["package*.json", "."]
COPY ["src/Search.UI/package*.json", "."]
RUN npm install
#COPY . .
COPY ./src/Search.UI/ .
RUN npm run build

# production stage
FROM nginx:stable-alpine as production-stage
COPY --from=build-stage /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]