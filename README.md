# 📸 Image Segmentation and Interactive Merging Tool

This project implements an image segmentation algorithm using graph-based region merging and includes an **interactive GUI** that allows users to **manually merge regions** by clicking on them. It's designed for educational and practical purposes — useful for understanding image processing, graph theory, and union-find (DSU) algorithms.

## 🧠 Overview

The segmentation is based on color similarity across RGB channels. It first applies **Gaussian smoothing** and then builds a **region adjacency graph** for each color channel.
Regions are segmented using **minimum spanning tree (MST)** and **union-find** with a threshold-based merging rule.

After segmentation, users can:

- **View color-coded segmented regions**
- **Click to select multiple segments**
- **Merge selected segments into one region**
- **Visualize the merged region in original colors**, while others turn white

## 🛠️ Features

- Graph-based segmentation using MST for red, green, and blue channels
- Custom `union-find` with size and internal difference tracking
- Real-time rendering of segmented regions
- GUI-based manual merging with click-to-select functionality
- Color-coded preview and reversion to original pixel values
- File output for region statistics (count and sizes)

## 🚀 How It Works

1. **Preprocessing**
   - Gaussian blur applied for smoothing.
2. **Graph Construction**
   - Each pixel connected to neighbors (right, down, diagonals).
   - Edge weights = absolute RGB differences.
3. **Segmentation**
   - Separate MSTs for red, green, blue channels.
   - Merging based on internal edge weights and size-based thresholds.
4. **Region Identification**
   - Final region ID based on `(redRoot, greenRoot, blueRoot)`.
   - BFS used to ensure spatial connectivity.
5. **Interactive Merging**
   - Click any pixel to select its region.
   - Selected regions are merged using their `parent` arrays.
   - GUI re-renders merged region in original colors; others appear white.
