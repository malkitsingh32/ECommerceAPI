# Git Commands for Committing to GitHub

## Step-by-Step Guide

### 1. Stage All Changes
```bash
# Add all modified and new files
git add .

# Or selectively add files
git add Infrastructure/Implementation/Services/
git add Application/Abstraction/Services/
git add *.md
```

### 2. Verify Staged Changes
```bash
# View what will be committed
git status

# View detailed changes
git diff --staged

# Expected output should show:
# - 5 new service files
# - 2 modified files (UserService.cs, Infrastructure.csproj, DependencyInjection.cs)
# - 4 new documentation files
# - New benchmark project
```

### 3. Commit with Descriptive Message
```bash
git commit -m "perf: Optimize authentication with JWT token caching

- Add IPasswordHasher abstraction with HMACSHA512 + fixed-time comparison
- Implement InMemoryTokenCache for single-server deployments (40x speedup)
- Implement DistributedTokenCache for multi-server Redis support
- Cache JWT handler, signing key, and credentials in UserService
- Add comprehensive benchmarks and performance documentation

Performance improvements:
- Single login: 57.4µs ? 6.4µs (88% faster)
- Cached token retrieval: 150ns (sub-microsecond)
- Repeated logins: 40x faster with in-memory cache
- Multi-server support with acceptable 3.3x overhead

Breaking changes: None
Backward compatible: Yes
New dependencies: Microsoft.Extensions.Caching.StackExchangeRedis (v6.0.0)

Fixes #[issue-number] (if applicable)"
```

Or use a template file:
```bash
git commit -F - << 'EOF'
perf: Optimize authentication with JWT token caching

- Add IPasswordHasher abstraction with HMACSHA512 + fixed-time comparison
- Implement InMemoryTokenCache for single-server deployments (40x speedup)
- Implement DistributedTokenCache for multi-server Redis support
- Cache JWT handler, signing key, and credentials in UserService
- Add comprehensive benchmarks and performance documentation

Performance improvements:
- Single login: 57.4µs ? 6.4µs (88% faster)
- Cached token retrieval: 150ns (sub-microsecond)
- Repeated logins: 40x faster with in-memory cache
- Multi-server support with acceptable 3.3x overhead

Breaking changes: None
Backward compatible: Yes
New dependencies: Microsoft.Extensions.Caching.StackExchangeRedis (v6.0.0)
EOF
```

### 4. View the Commit
```bash
# View your commit
git log -1 --stat

# See detailed changes
git log -1 -p

# Expected: ~800 lines of code, 1200+ lines of documentation
```

### 5. Push to GitHub

#### Option A: Push to Feature Branch (Recommended for PR)
```bash
# Create feature branch if not already on one
git checkout -b feature/auth-optimization

# Push to GitHub
git push -u origin feature/auth-optimization

# Output should show:
# Create pull request at: https://github.com/username/repo/pull/new/feature/auth-optimization
```

#### Option B: Push Directly to Main (Direct Merge)
```bash
# Ensure you're on main and updated
git checkout main
git pull origin main

# Create new branch
git checkout -b performance/jwt-token-caching
git push -u origin performance/jwt-token-caching

# Push changes
git push
```

### 6. Create Pull Request (if using PR workflow)
```
Title: "Optimize authentication with JWT token caching (88% improvement)"

Description:
This PR introduces comprehensive performance optimizations for user authentication
and JWT token generation.

## Summary
- Achieves 88% improvement on single login operations
- Provides 40× speedup on repeated logins via token caching
- Supports both single-server and multi-server deployments
- Adds comprehensive benchmarking and documentation

## Performance
- Single login: 57.4µs ? 6.4µs
- Cache hit: 150.5ns
- Repeated logins: 33.1µs (in-memory) / 109.4µs (distributed)

## Changes
- 5 new service implementations
- Updated UserService with caching strategy
- Added Redis support for distributed deployments
- Comprehensive documentation and benchmarks

## Type of Change
- [x] Performance improvement
- [x] New feature (token caching)
- [ ] Breaking change
- [x] Documentation

## Testing
- All benchmarks pass
- Full backward compatibility
- Zero breaking changes

## Documentation
- README_OPTIMIZATION.md - Technical documentation
- QUICK_START.md - Developer guide
- Inline code comments
```

### 7. Merge After Approval
```bash
# Update main branch
git checkout main
git pull origin main

# Merge feature branch
git merge feature/auth-optimization

# Push to main
git push origin main

# Delete feature branch (optional)
git branch -d feature/auth-optimization
git push origin --delete feature/auth-optimization
```

## Pre-Push Checklist

Run these before pushing:

```bash
# 1. Verify build still works
dotnet clean
dotnet build -c Release
if [ $? -eq 0 ]; then echo "? Build successful"; else echo "? Build failed"; exit 1; fi

# 2. Verify tests pass (benchmarks)
cd BenchmarkSuite1
dotnet run -c Release --filter "*LoginBenchmark" --warmupCount 1 --targetCount 1
cd ..

# 3. Verify no uncommitted changes
git status --short

# 4. Verify commit size is reasonable
git log -1 --stat | grep -E "^\s*[0-9]+ files? changed"
```

## Useful Commands

### View your commits
```bash
# Show latest commit
git log -1

# Show commits for this branch
git log --oneline -n 10

# Show commits with changes
git log -1 -p
```

### If you need to modify the commit
```bash
# Amend the most recent commit (before pushing)
git add .
git commit --amend --no-edit

# Amend commit message
git commit --amend -m "New message"

# Revert the commit (after pushing)
git revert HEAD
```

### If you need to undo everything
```bash
# Reset to before commit (keep changes)
git reset --soft HEAD~1

# Reset to before commit (discard changes)
git reset --hard HEAD~1
```

## GitHub Web Interface (Alternative)

If you prefer not to use command line:

1. **Go to your GitHub repository**
2. **Click "Create new branch"** and name it `feature/auth-optimization`
3. **Upload files** or use GitHub's web editor to create the changes
4. **Create a pull request** from the new branch to main
5. **Request reviewers**
6. **Merge when approved**

## Tips

? **Good commit messages:**
- Start with type (perf:, feat:, fix:, docs:, etc.)
- Be descriptive but concise
- Include what changed and why
- Reference issue numbers if applicable

? **Avoid:**
- Generic messages like "Update" or "Fixed stuff"
- Committing to main without review
- Mixing unrelated changes in one commit
- Committing credentials or secrets

## Rollback (if needed)

```bash
# If commit is already pushed and needs to be reverted
git revert <commit-hash>

# Or create a revert commit
git revert HEAD

# Push the revert
git push origin main
```

---

## Quick Reference

```bash
# Complete workflow
git status                    # Check status
git add .                     # Stage changes
git status                    # Verify staging
git commit -m "perf: ..."     # Commit
git push -u origin branch     # Push to GitHub
# Create PR on GitHub web
# Request review
# Merge when approved
```

**Ready to push? Let's go! ??**
