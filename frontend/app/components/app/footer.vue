<template>
  <footer>
    <div class="left">
      <p>&copy; Split the bill 2025</p>
    </div>

    <div class="right">
      <AppColorModeButton />
      <UButton
        to="https://github.com/butskristof/split-the-bill"
        target="_blank"
        icon="i-mynaui-brand-github"
        aria-label="GitHub"
        color="neutral"
        variant="ghost"
      />
    </div>
  </footer>
</template>

<style scoped>
footer {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;

  margin-top: 1rem;
  margin-bottom: 2rem;
}
</style>
