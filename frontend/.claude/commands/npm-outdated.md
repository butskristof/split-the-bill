# Update npm Dependencies

Check for outdated npm packages and intelligently apply updates based on semver:

1. **Patch updates** (x.x.PATCH): Apply automatically - these are bug fixes only
2. **Minor updates** (x.MINOR.x): Research breaking changes and new features before applying
3. **Major updates** (MAJOR.x.x): Review migration docs and release notes, then help plan the upgrade

## Process

1. Run `npm outdated` to check for available updates
2. Categorize updates by semver level (patch/minor/major)
3. For patch updates:
   - Apply all patch updates with `npm install package@version`
   - Verify package.json was updated
4. For minor updates:
   - Search for release notes/changelog for each package
   - Check for breaking changes or notable new features
   - Summarize findings and recommend whether to apply
   - If approved, apply the update
5. For major updates:
   - Find and review migration guides or release notes
   - Identify breaking changes and required code changes
   - Create a migration plan with specific steps
   - If approved, apply the update and help with code changes
6. After all updates:
   - Run `npm run test:ts` to verify TypeScript types
   - Report summary of all changes made

## Notes

- Always use `npm install package@version` to ensure package.json is updated
- Check for multiple packages that should be updated together (e.g., @regle/\* packages)
- For breaking changes, provide code examples of what needs to change
- If documentation is unclear, use web search to find release notes
