# NuGet.Extras
This library is aimed at providing a bunch of additional base functionality for use in NuGet console extensions.

## Features
Currently they are a bit anaemic, however some of the functionality that can be used:

* Classes to aggregate unique package references across a solution, reducing the number of packages to restore.  This also provides the smallest set of packages based on version constraints
* Classes to provide non-version specific upgrade of packages
* Classes to allow searching for a particular _.dll_ in a feed

## Usage
Many of the classes are just general plumbing that are used in additional command line extensions.  Where it makes sense as a reusable component, functionality will be moved into this package.

## Testing
Working on it.

## Examples
See above.