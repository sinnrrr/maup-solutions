#!/bin/bash

# Directory containing the DOC/DOCX files
directory=$1

# Get the directory path of the script
script_dir=$(dirname "$0")

# CSS file for styling
css_file="$script_dir/styles.css"

# Loop through DOC files and convert them to docx
for file in "$directory"/*.doc; do
    if [[ -f "$file" ]]; then
        doc2docx "$file"
        echo "Converted DOC to DOCX: $file"
    fi
done

# Loop through DOCX files in the directory
for file in "$directory"/*.docx; do
    if [[ -f "$file" ]]; then
        # Get the file name and extension
        file_name="${file%.*}"
        extension="${file##*.}"

        # Generate the modified output file name
        output_file="${file_name}_modified.$extension"

        # Apply pandoc command with CSS styling
        pandoc -s "$file" -o "$output_file" --css "$css_file"

        echo "Modified file created: $output_file"
    fi
done
