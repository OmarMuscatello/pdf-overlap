# pdf-overlap
Overlaps each page of a PDF file to each page of another PDF file using [iTextSharp](https://sourceforge.net/projects/itextsharp/).

## Installation
- Clone or download this repository
- Set values of `baseFilePath`, `overFilePath` and `outputFilePath`.
- Run!

## Constants explaination
| Constant name    | Description                                                                                                                        |
|------------------|------------------------------------------------------------------------------------------------------------------------------------|
| `baseFilePath`   | Path of the file to be overlapped. Each page of this file will be overlapped by the corrisponding page of `overFilePath` PDF file. |
| `overFilePath`   | Path of the file to overlap. Each page of this file will overlap the corrisponding page of `baseFilePath` PDF file.                |
| `outputFilePath` | The output file path: `overFilePath` pages over `baseFilePath` pages.                                                              |