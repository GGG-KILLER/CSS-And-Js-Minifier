# CSS-And-Js-Minifier
C# Program to minify all JavaScript and CSS files inside a folder.

**It doesn't uses makes use of cssminifier.com and javascript-minifier.com anymore.**

If you really want it to use it, you can use [v1.1](https://github.com/GGG-KILLER/CSS-And-Js-Minifier/releases/tag/v1.1)

Without the dependency of online services, it can be used offline and it also is much more faster than the previous versions.

Current version: 2.0.0

# What does it do?
It searches for all the css and js files inside it ignoring the ones with .min.js or .min.css then minifies the file to {filename}.min.{ext}.

# Used libraries:
- [YUI Compressor.NET](https://github.com/PureKrome/YUICompressor.NET) (provides the minification methods)
- [Costura.Fody](https://github.com/Fody/Costura) (embbeds all DLLs inside the main exe)

# License
CSS & Js Minifier
Copyright (C) 2015  GGG KILLER

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
