FROM node:14.18.1 AS builder

WORKDIR /app

COPY . .

RUN npm install
RUN npm run build

FROM nginx:alpine

COPY --from=builder /app/dist/* /usr/share/nginx/html/