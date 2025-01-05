FROM node:23.5-bookworm-slim AS build
WORKDIR /build
COPY package.json .
RUN npm install
COPY . .
RUN npm run build

FROM node:23.5-bookworm-slim AS final
WORKDIR /app
COPY --from=build /build/dist .
RUN npm install -g serve
CMD ["serve", "-s"]