FROM node:20-alpine AS build
WORKDIR /app

# Copy package.json and install dependencies
COPY package*.json ./
RUN npm install --legacy-peer-deps

# Copy all source files
COPY . .

# Build Angular project
RUN npm run build-prod

# Expose runtime port
EXPOSE 4000

# Serve the built static site from the Angular output path
# angular.json config uses: dist/wwwroot/website
CMD ["npx", "serve", "-s", "dist/wwwroot/website/browser", "-l", "4000"]

