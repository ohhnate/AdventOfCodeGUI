// -----------------------------------------------------------
// Advent of Code 2024 - GUI Interface
// Interactive interface for running AoC solutions
// -----------------------------------------------------------

using System.Diagnostics;

namespace Advent2024;

public class AdventForm : Form
{
    private CheckedListBox _daysListBox;
    private Button _runSelectedButton;
    private Button _runAllButton;
    private Button _selectAllButton;
    private Button _clearSelectionButton;
    private Button _cancelButton;
    private TextBox _outputTextBox;
    private ProgressBar _progressBar;
    private TableLayoutPanel _mainLayout;
    private FlowLayoutPanel _buttonPanel;
    private CancellationTokenSource? _cancellationTokenSource;

    public AdventForm()
    {
        InitializeComponents();
        StyleComponents();
    }

    private void InitializeComponents()
    {
        Text = "Advent of Code GUI";
        Size = new Size(800, 600);
        MinimumSize = new Size(600, 400);

        // Main layout
        _mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(10),
            ColumnCount = 2,
            RowCount = 3
        };
        // Set column widths
        _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
        _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        // Day selection list
        _daysListBox = new CheckedListBox
        {
            Dock = DockStyle.Fill,
            CheckOnClick = true,
            BorderStyle = BorderStyle.FixedSingle
        };
        for (int i = 1; i <= 25; i++)
        {
            _daysListBox.Items.Add($"Day {i}", false);
        }
        // Button panel
        _buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            AutoSize = true
        };

        _selectAllButton = new Button
        {
            Text = "Select All",
            AutoSize = true,
            Padding = new Padding(10, 5, 10, 5)
        };
        _selectAllButton.Click += (s, e) => SelectAll(true);
        _clearSelectionButton = new Button
        {
            Text = "Clear All",
            AutoSize = true,
            Padding = new Padding(10, 5, 10, 5)
        };
        _clearSelectionButton.Click += (s, e) => SelectAll(false);
        _runSelectedButton = new Button
        {
            Text = "Run Selected",
            AutoSize = true,
            Padding = new Padding(10, 5, 10, 5)
        };
        _runSelectedButton.Click += RunSelectedButton_Click;
        _runAllButton = new Button
        {
            Text = "Run All Days",
            AutoSize = true,
            Padding = new Padding(10, 5, 10, 5)
        };
        _runAllButton.Click += RunAllButton_Click;
        _cancelButton = new Button
        {
            Text = "Cancel",
            AutoSize = true,
            Visible = false,
            Padding = new Padding(10, 5, 10, 5)
        };
        _cancelButton.Click += CancelButton_Click;
        _buttonPanel.Controls.AddRange([
            _selectAllButton, 
            _clearSelectionButton,
            _runSelectedButton,
            _runAllButton,
            _cancelButton
        ]);
        // Progress bar
        _progressBar = new ProgressBar
        {
            Dock = DockStyle.Fill,
            Visible = false,
            Style = ProgressBarStyle.Continuous
        };
        // Output text box
        _outputTextBox = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            ReadOnly = true,
            Font = new Font("Cascadia Code", 9F),
            BorderStyle = BorderStyle.FixedSingle
        };

        // Add controls to layout
        _mainLayout.Controls.Add(_daysListBox, 0, 0);
        _mainLayout.Controls.Add(_buttonPanel, 1, 0);
        _mainLayout.Controls.Add(_progressBar, 0, 1);
        _mainLayout.SetColumnSpan(_progressBar, 2);
        _mainLayout.Controls.Add(_outputTextBox, 0, 2);
        _mainLayout.SetColumnSpan(_outputTextBox, 2);

        // Set row styles
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Controls.Add(_mainLayout);
    }

    private void StyleComponents()
    {
        BackColor = Color.FromArgb(45, 45, 48);
        ForeColor = Color.White;

        Action<Button> buttonStyle = btn =>
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.FromArgb(63, 63, 70);
            btn.ForeColor = Color.White;
            btn.FlatAppearance.BorderColor = Color.FromArgb(100, 100, 100);
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(73, 73, 80);
        };
        buttonStyle(_runSelectedButton);
        buttonStyle(_runAllButton);
        buttonStyle(_cancelButton);
        buttonStyle(_selectAllButton);
        buttonStyle(_clearSelectionButton);
        _daysListBox.BackColor = Color.FromArgb(30, 30, 30);
        _daysListBox.ForeColor = Color.White;
        _outputTextBox.BackColor = Color.FromArgb(30, 30, 30);
        _outputTextBox.ForeColor = Color.White;
    }

    private void SelectAll(bool check)
    {
        for (int i = 0; i < _daysListBox.Items.Count; i++)
        {
            _daysListBox.SetItemChecked(i, check);
        }
    }

    private async void RunSelectedButton_Click(object? sender, EventArgs e)
    {
        List<int> selectedDays = _daysListBox.CheckedIndices.Cast<int>().Select(i => i + 1).ToList();
        if (selectedDays.Count == 0)
        {
            MessageBox.Show("Please select at least one day to run.", "No Days Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        await RunDays(selectedDays);
    }

    private async void RunAllButton_Click(object? sender, EventArgs e)
    {
        await RunDays(Enumerable.Range(1, 25).ToList());
    }

    private void CancelButton_Click(object? sender, EventArgs e)
    {
        _cancellationTokenSource?.Cancel();
    }

    private async Task RunDays(List<int> days)
    {
        SetControlsRunningState(true);
        _progressBar.Maximum = days.Count;
        _progressBar.Value = 0;
        _outputTextBox.Clear();
        TimeSpan totalExecutionTime = TimeSpan.Zero;
        try
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await Task.Run(async () =>
            {
                TextWriter originalOutput = Console.Out;
                await using StringWriter writer = new();
                Console.SetOut(writer);
                bool anySuccess = false;
                int progress = 0;
                foreach (int day in days)
                {
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    Type? solverType = Type.GetType($"Advent2024.Days.Day{day}");
                    if (solverType == null) continue;
                    if (Activator.CreateInstance(solverType) is BaseDaySolver solver)
                    {
                        anySuccess = true;
                        Stopwatch dayStopwatch = Stopwatch.StartNew();
                        Console.WriteLine($"=== Day {day} ===");
                        dayStopwatch.Restart();
                        object part1Result = solver.SolvePart1();
                        TimeSpan part1Time = dayStopwatch.Elapsed;
                        Console.WriteLine($"Part 1: {part1Result} (Time: {part1Time.TotalMilliseconds:F2}ms)");
                        _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                        dayStopwatch.Restart();
                        
                        object part2Result = solver.SolvePart2();
                        TimeSpan part2Time = dayStopwatch.Elapsed;
                        Console.WriteLine($"Part 2: {part2Result} (Time: {part2Time.TotalMilliseconds:F2}ms)");
                        
                        TimeSpan dayTotalTime = part1Time + part2Time;
                        totalExecutionTime += dayTotalTime;
                        Console.WriteLine($"Total Time: {dayTotalTime.TotalMilliseconds:F2}ms\n");
                    }
                    progress++;
                    Invoke(() => _progressBar.Value = progress);
                    await Task.Delay(1);
                }
                Console.WriteLine();  // Add extra space before total
                Console.WriteLine(anySuccess
                    ? $"Total execution time: {totalExecutionTime.TotalSeconds:F3} seconds"
                    : "No solutions found!");
                Console.SetOut(originalOutput);
                Invoke(() => _outputTextBox.Text = writer.ToString());
            }, _cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            _outputTextBox.Text += "\r\nOperation cancelled by user.";
        }
        finally
        {
            SetControlsRunningState(false);
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }

    private void SetControlsRunningState(bool isRunning)
    {
        _daysListBox.Enabled = !isRunning;
        _runSelectedButton.Enabled = !isRunning;
        _runAllButton.Enabled = !isRunning;
        _selectAllButton.Enabled = !isRunning;
        _clearSelectionButton.Enabled = !isRunning;
        _cancelButton.Visible = isRunning;
        _progressBar.Visible = isRunning;
    }
}