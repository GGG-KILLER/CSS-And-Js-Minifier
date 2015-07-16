# CSS-And-Js-Minifier
Simple C# Program that makes use of cssminifier.com and javascript-minifier.com to minify and compress all CSS and Js files on a folder

# What does it do?
It searches for all the css and js files inside it ignoring the ones with .min.js or .min.css then compresses the original to a .gz file inscase your server has gzip enabled and also minifies the file to filename.min.ext and gzips it too.

## Why does it gzips?
Because I use pre-gzipped files on my web server and since I created this tool to use on it I added this functionality.

## Is there a way to disable gzipping?
Maybe in a future version or if I get too many requests.

# License
This program is distributed under the MIT License.

The tools used to minify the files are not my responsability and are provided by their respective creators.
This program is given without ANY warranty nor support so please do not contact me asking for support.
