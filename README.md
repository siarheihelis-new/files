# Large File Sorting Solution

A high-performance C# solution for generating and sorting large text files efficiently, designed to handle files up to 100GB

## Overview

This solution consists of three components:
- **Generator**: Creates test files with lines in the format `<Number>. <String>`
- **Sorter**: Efficiently sorts large files using external merge sort
- **Tests**: Comprehensive unit tests and performance benchmarks

## Sorting Criteria

1. **Primary**: Sort by the string part (alphabetically)
2. **Secondary**: Sort by the number (ascending) when strings are identical

### Example

**Input:**
```
415. Apple
30432. Something something something
1. Apple
32. Cherry is the best
2. Banana is yellow
```

**Output:**
```
1. Apple
415. Apple
2. Banana is yellow
32. Cherry is the best
30432. Something something something
```

## Components

### 1. File Generator (`Helis.Files.Generator`)

Generates test files with configurable size.

**Usage:**
```bash
dotnet run --project Helis.Files.Generator [output_file] [size]
```

**Examples:**
```bash
# Generate 1GB file (default)
dotnet run --project Helis.Files.Generator large_file.txt

# Generate 10GB file
dotnet run --project Helis.Files.Generator test.txt 10GB

# Generate custom size in bytes
dotnet run --project Helis.Files.Generator test.txt 1000000000
```

**Features:**
- Configurable file size
- Ability to specify percent of duplicated <String> part

### 2. File Sorter (`Helis.Files.Sorter`)

Sorts files using external merge sort algorithm.

**Usage:**
```bash
dotnet run --project Helis.Files.Sorter [input_file] [output_file] [max_memory_MB]
```

**Examples:**
```bash
# Sort with default 32MB memory limit (per chunk)
dotnet run --project Helis.Files.Sorter large_file.txt sorted.txt