terraform {
  required_providers {
    docker = {
      source  = "kreuzwerker/docker"
      version = "~> 3.0.2"
    }
  }
}

provider "docker" {
  host = "npipe:////.//pipe//docker_engine"
}

// Definindo a imagem
resource "docker_image" "nginx" {
  name         = "nginx"
  keep_locally = false
}


// Definindo o Container
resource "docker_container" "nginx" {
  image = docker_image.nginx.image_id
  name  = "web-server"

  ports {
    internal = 80
    external = 8000
  }
}