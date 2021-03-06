# Getting started

## Introduction

A sample dataset with some 3D buildings is provided, see [sample_data/buildings.backup](sample_data/buildings.backup). The coordinate system of these buildings is earth centred, earth fixed (EPSG:4978). 

In this document we run pg2b3dm on this dataset and visualize the buildings in Cesium and MapBox GL JS.

## Run pg2b3dm

Steps to get pg2b3dm running on this sample dataset:

0] Create Docker network

In this tutorial, we'll start 2 containers: PostGIS database and tiling tool pg2b3dm. Because those containers need to communicate
they must be in the same network. So we'll create a network first and add the 2 containers later.

```
$ docker network create  mynetwork
```

1] Start PostGIS database

```
$ docker run --name some-postgis -e POSTGRES_PASSWORD=postgres -p 5432:5432 -it --network mynetwork mdillon/postgis
```

2] Connect to database using pgAdmin or similar db management tool

3] Create schema 'bertt'

4] Select PostgreSQL database and restore file buildings.backup

A table bertt.buildings will be created, contains 100 sample buildings in Amsterdam.

5] Run pg2b3dm, the program will make a connection to the database and 1 tileset.json and 9 b3dm's will be created in the output directory.

```
λ docker run -v $(pwd)/output:/app/output -it --network mynetwork geodan/pg2b3dm -h some-postgis -U postgres -c geom -t  bertt.buildings -d postgres -r colors -a blockid
tool: pg2b3dm 0.5.1.0
Password for user postgres:
Start processing....
Calculating bounding boxes...
Writing tileset.json...
Writing 9 tiles...
Progress: tile 9 - 100.00%
Elapsed: 2 seconds
Program finished.
```

## Visualize in Cesium

Copy the generated tiles to sample_data\cesium (overwrite the sample tiles there).

Put [sample_data/index_cesium.html](sample_data/index_cesium.html) on a webserver (for example https://caddyserver.com/).

Open a browser and if all goes well In Amsterdam you can find some 3D Tiles buildings:

![Hello World Buildings](https://user-images.githubusercontent.com/538812/63441248-6517a200-c431-11e9-96c5-d1d38d2513a6.png)

Right click on a building and check browser console to see the batching option on column 'blockid' at work.

## Visualize in MapBox GL JS

To visualize in MapBox GL JS we have to transform the buildings table to Spherical Mercator (3857):

```
CREATE TABLE bertt.buildings_3857 AS 
SELECT ST_Transform(geom,3857) AS geom, blockid,color, colors 
FROM bertt.buildings;
```

And rerun pg2b3dm on this table:

```
$ docker run -v $(pwd)/output:/app/output -it --network mynetwork geodan/pg2b3dm -h some-postgis -U postgres -c geom -t  bertt.buildings_3857 -d postgres -r colors -a blockid -i blockid
```

The b3dm tiles can be visualized by adding Three.JS and glTF functionality to the MapBox GL JS viewer. See https://github.com/Geodan/mapbox-3dtiles for more information about this topic.

Put sample html page [sample_data/index_mapbox.html](sample_data/index_mapbox.html) on a webserver next to the generated tileset.json, and copy https://github.com/Geodan/mapbox-3dtiles/blob/master/Mapbox3DTiles.js to this folder.

Final result in MapBox GL JS:

![hoofden](https://user-images.githubusercontent.com/538812/63675318-d320e800-c7e8-11e9-82f4-fcfb2a187044.png)

## Customize building colors

Change some colors in the 'colors' column and run pg2b3m again. Restart Cesium and the new colors should be visible.

