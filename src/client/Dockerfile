# pull official base image
FROM node:16.3.0-alpine as build

# set working directory
WORKDIR /app

# add `/app/node_modules/.bin` to $PATH
ENV PATH /app/node_modules/.bin:$PATH

# install app dependencies
COPY package.json ./
COPY package-lock.json ./
RUN npm ci --silent --legacy-peer-deps

COPY . ./

RUN npm run build

# production environment
FROM nginx:stable-alpine

COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
