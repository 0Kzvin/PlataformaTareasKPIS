
import os
import unicodedata
import re
import csv

# Configuration
KEYS_FILE = "keys.txt"
CSV_FILE = "../../translations.csv"  # Relative to scripts/translation_utils

def normalize_key(text):
    """
    Converts a text string into a clean CamelCase key.
    - Removes accents (á -> a)
    - Removes non-alphanumeric characters (except spaces initially)
    - Converts to CamelCase (Removing Spaces)
    Example: "Gestión de Altas" -> "GestionDeAltas"
    """
    if not text:
        return ""
    # Normalize unicode characters (accents)
    text = unicodedata.normalize('NFKD', text).encode('ASCII', 'ignore').decode('utf-8')
    
    # Remove special chars but keep spaces for Title Case conversion
    text = re.sub(r'[^a-zA-Z0-9\s]', '', text)
    
    # Split by space and capitalize each word
    words = text.split()
    camel_case = "".join(word.capitalize() for word in words)
    
    return camel_case

def load_keys_from_txt(path):
    if not os.path.exists(path):
        return []
    with open(path, 'r', encoding='utf-8') as f:
        # Read lines, strip whitespace, ignore empty lines
        return [line.strip() for line in f if line.strip()]

def load_csv(path):
    existing_keys = {}
    lines = []
    header = None
    
    try:
        with open(path, 'r', encoding='utf-8') as f:
            lines = f.readlines()
    except FileNotFoundError:
        pass
    except UnicodeDecodeError:
        with open(path, 'r', encoding='latin-1') as f:
            lines = f.readlines()

    if lines:
        header = lines[0].strip()
        # Simple CSV parsing for keys
        # Assuming Format: Key,"Lang1","Lang2"...
        # We split by comma but respect quotes roughly for the first column
        for line in lines[1:]:
            parts = line.split(',')
            if parts:
                key = parts[0].strip().replace('"', '')
                existing_keys[key] = line.strip()
                
    return header, lines, existing_keys

def main():
    print("--- Unified Translation Manager ---")
    
    current_dir = os.path.dirname(os.path.abspath(__file__))
    keys_path = os.path.join(current_dir, KEYS_FILE)
    csv_path = os.path.join(current_dir, CSV_FILE)
    
    print(f"Loading keys from: {keys_path}")
    input_strings = load_keys_from_txt(keys_path)
    
    print(f"Loading CSV from: {csv_path}")
    header, existing_csv_lines, existing_keys_map = load_csv(csv_path)
    
    if not header:
        print("Error: CSV does not have a header or is empty.")
        return

    # Determine number of columns based on header
    # Header example: Key,"Spanish (Mexico), es_MX","English (US), en-US"
    # We can split by comma to count. 
    # NOTE: The header in this project seems to have quoted fields.
    # We'll assume the number of language columns = Total Columns - 1 (Key column)
    header_cols = header.split(',')
    # Logic: If header is `Key,"Val1, code","Val2, code"`, split might be complex due to commas inside quotes.
    # But usually language files have a standard structure.
    # Let's count how many commas + 1.
    # Or properly parse the CSV line.
    
    try:
        reader = csv.reader([header])
        header_parsed = list(reader)[0]
        num_columns = len(header_parsed)
    except:
        # Fallback
        num_columns = 3 # Key, Spanish, English default
        print("Warning: Could not check header columns, defaulting to 3.")

    new_entries = []
    
    for text_source in input_strings:
        final_key = normalize_key(text_source)
        
        if not final_key:
            continue
            
        if final_key in existing_keys_map:
            # Key Exists
            pass
        else:
            print(f"[NEW] Adding key: {final_key} ('{text_source}')")
            
            # Construct the row
            # Col 0: Key
            # Col 1: Source Text (Spanish)
            # Col 2+: Placeholders ("")
            
            row_values = []
            row_values.append(final_key)
            row_values.append(text_source) # Assuming 1st lang is source (Spanish)
            
            # Fill remaining columns with empty strings
            while len(row_values) < num_columns:
                row_values.append("") 
                
            # Convert to CSV string line
            # Escape double quotes
            safe_values = [f'"{val.replace('"', "'")}"' for val in row_values]
            line = ",".join(safe_values)
            
            new_entries.append(line)
            
    if new_entries:
        with open(csv_path, 'a', encoding='utf-8', newline='') as f:
            if existing_csv_lines and not existing_csv_lines[-1].endswith('\n'):
                f.write('\n')
            for entry in new_entries:
                f.write(entry + '\n')
        print(f"Successfully added {len(new_entries)} new keys.")
    else:
        print("No new keys to add.")
        
if __name__ == "__main__":
    main()
