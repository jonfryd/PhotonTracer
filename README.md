[![Java 5](https://img.shields.io/badge/Java-5-blue.svg)](http://www.oracle.com/technetwork/java/javase)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

# ImageClustering: Content-Based Image File Renaming Tool

This is a hobby project that I did back in 2008 based on JAI (Java Advanced Imaging API).

It purpose is to take a directory of randomly named image files (e.g. JPEG or PNG) and bring order to chaos
by using classic image feature extraction techniques along with a [self-organizing map for clustering](http://somlib.gforge.inria.fr/).

A couple of features which be useful such as multi-threaded processing and distributing work over the network
to different nodes. It also comes with a basic Swing UI:

![ImageClustering Swing UI](ui.jpg?raw=true)

# Building

Clone the repository and execute Maven from the root directory:

    $ git clone https://github.com/jonfryd/ImageClustering
    $ cd ImageClustering/
    $ mvn clean install

# Usage

After building, run e.g. the UI as:

    $ ./runImageClustering.sh

This is for Linux or Mac OS X. For Windows run .bat-file:

    $ runImageClustering.bat

When the UI is launched, select the directory, enter the expected number of clusters and press 'Process'.
Images are then analyzed and renamed with a C###-prefix denoting the cluster each image belongs to.

Happy renaming.

# Author

This application created by Jon Frydensbjerg - email: jonf@elixlogic.com
