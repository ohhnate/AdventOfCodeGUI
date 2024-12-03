# Advent of Code 2024 w/GUI

My solutions for [Advent of Code 2024](https://adventofcode.com/2024) in C#.
I will be updating the solutions, helpers, and GUI while the challenge advances as needed.

## Project Structure

```
Advent2024/
├── Days/           # Solution for each day
│   ├── Day1.cs
│   └── ...
├── Inputs/         # Inputs for each day
│   ├── day1input.txt
│   └── ...
├── Helpers/        # Utility classes
│   ├── FileHelper.cs
│   ├── GraphHelper.cs
│   ├── GridHelper.cs
│   └── MathHelper.cs
└── README.md
```

## Features

- Modern C# (.NET 8.0)
- GUI interface for running solutions
- Command-line support
- Helper utilities for common AoC operations
- Performance timing for solutions

## Usage

### GUI Mode

Run the application without arguments to launch the GUI interface:

```bash
dotnet run
```

### Command Line Mode

Run a specific day's solution:

```bash
dotnet run -- <day>
```

Example:

```bash
dotnet run -- 1
```

## Helper Utilities

### FileHelper

- File reading operations
- Multi-column data parsing
- Grid parsing
- JSON parsing
- Group reading

### MathHelper

- GCD/LCM calculations
- Prime number operations
- Manhattan distance
- Range operations

## License

This project is open source and available under the [MIT License](LICENSE).