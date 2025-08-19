#!/usr/bin/env python3
"""
Asset Synchronization Tool for Godot Projects

This script synchronizes assets from a source directory to the project's assets directory,
handling Godot's .import files and maintaining directory structure.
"""

import os
import shutil
import sys
from pathlib import Path

def sync_assets(source_dir, assets_dir):
    """
    Synchronize files from source_dir to assets_dir.
    
    Args:
        source_dir (str): Source directory containing the assets
        assets_dir (str): Target assets directory (usually project's assets folder)
    """
    source_path = Path(source_dir).resolve()
    assets_path = Path(assets_dir).resolve()
    
    # Ensure source directory exists
    if not source_path.exists() or not source_path.is_dir():
        print(f"Error: Source directory does not exist: {source_path}")
        return False
    
    # Ensure assets directory exists
    assets_path.mkdir(parents=True, exist_ok=True)
    
    # Track all processed files and directories to identify what to remove
    processed_files = set()
    processed_dirs = set()
    
    # Walk through source directory and copy files
    for root, dirs, files in os.walk(source_path):
        # Get relative path from source directory
        rel_path = Path(root).relative_to(source_path)
        target_dir = assets_path / rel_path
        
        # Create target directory if it doesn't exist
        target_dir.mkdir(parents=True, exist_ok=True)
        
        # Add this directory to processed_dirs
        processed_dirs.add(str(rel_path))
        
        # Process each file in the current directory
        for file in files:
            if file == '.DS_Store':  # Skip macOS system files
                continue
                
            source_file = Path(root) / file
            target_file = target_dir / file
            
            # Skip .import files in source directory
            if source_file.suffix == '.import':
                continue
                
            # Check if we need to copy the file
            copy_needed = True
            if target_file.exists():
                # If target exists and is newer than source, skip
                if target_file.stat().st_mtime >= source_file.stat().st_mtime:
                    copy_needed = False
            
            if copy_needed:
                print(f"Copying: {source_file} -> {target_file}")
                shutil.copy2(source_file, target_file)
            
            # Mark this file as processed
            processed_files.add(str(rel_path / file))
            
            # Handle .import file
            import_file = target_file.parent / f"{file}.import"
            if not import_file.exists():
                # If no .import file exists, create an empty one
                import_file.touch()
    
    # First pass: Process all files and .import files
    for root, dirs, files in os.walk(assets_path, topdown=False):
        rel_root = Path(root).relative_to(assets_path)
        
        # Check files
        for file in files:
            if file == '.DS_Store':  # Skip macOS system files
                continue
                
            if file.endswith('.import'):
                # Check if the corresponding asset file exists
                asset_file = Path(root) / file[:-7]  # Remove .import extension
                if not asset_file.exists():
                    print(f"Removing orphaned .import file: {asset_file}")
                    os.remove(Path(root) / file)
                continue
                
            rel_path = str(rel_root / file)
            if rel_path not in processed_files:
                file_path = Path(root) / file
                print(f"Removing: {file_path}")
                os.remove(file_path)
                
                # Also remove corresponding .import file if it exists
                import_file = file_path.parent / f"{file}.import"
                if import_file.exists():
                    print(f"Removing: {import_file}")
                    os.remove(import_file)
        
    # Second pass: Handle directories
    # First, collect all directories in assets
    all_assets_dirs = set()
    for root, dirs, _ in os.walk(assets_path):
        rel_root = Path(root).relative_to(assets_path)
        for dir_name in dirs:
            all_assets_dirs.add(str(rel_root / dir_name) if rel_root != Path('.') else dir_name)
    
    # Remove directories that don't exist in source
    dirs_to_remove = all_assets_dirs - processed_dirs
    for dir_rel in sorted(dirs_to_remove, key=lambda x: x.count(os.sep), reverse=True):
        dir_path = assets_path / dir_rel
        try:
            dir_path.rmdir()  # Will only remove if empty
            print(f"Removed directory not in source: {dir_path}")
            # Also remove corresponding .import directory if it exists
            import_dir = dir_path.parent / f"{dir_path.name}.import"
            if import_dir.exists() and import_dir.is_dir():
                shutil.rmtree(import_dir)
                print(f"Removed .import directory: {import_dir}")
        except OSError as e:
            print(f"Note: Could not remove directory {dir_path}: {e}")
    
    # Make sure all processed directories exist in assets
    for dir_rel in processed_dirs:
        if dir_rel == '.':
            continue
        dir_path = assets_path / dir_rel
        dir_path.mkdir(parents=True, exist_ok=True)
    
    return True

def main():
    if len(sys.argv) != 2:
        print(f"Usage: {sys.argv[0]} <source_directory>")
        print("  This will sync files from <source_directory> to the assets/ directory")
        return 1
    
    source_dir = sys.argv[1]
    assets_dir = os.path.join(os.path.dirname(os.path.dirname(os.path.abspath(__file__))), 'assets')
    
    print(f"Synchronizing assets from: {source_dir}")
    print(f"                     to: {assets_dir}")
    
    if sync_assets(source_dir, assets_dir):
        print("Synchronization completed successfully.")
        return 0
    else:
        print("Synchronization failed.")
        return 1

if __name__ == "__main__":
    sys.exit(main())
