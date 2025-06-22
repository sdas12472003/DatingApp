# Stage 1: Build Angular app
FROM node:20 AS builder
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build --configuration production

# Stage 2: Serve app with NGINX
FROM nginx:alpine
COPY --from=builder /app/dist/DatingApp /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
